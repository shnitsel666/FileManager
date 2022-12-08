using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using LanDocs.Indexing.IntegrationTests.Configuration;
using LanDocs.Indexing.IntegrationTests.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Xunit;

namespace LanDocs.Indexing.IntegrationTests
{
    public class IndexingTest
    {
        #region Helpers

        private static class IndexingIntegrationTestsUri
        {
            public const string Execute = "api/v1/tests/execute";

            public const string Result = "api/v1/tests/result";
        }

        private class GetTestsResultRequest
        {
            public Guid JobId { get; set; }
        }

        private class GetTestsResultResponse
        {
            public IReadOnlyCollection<TestResult> Result { get; set; }
        }

        private class PostExecuteTestsResponse
        {
            public Guid JobId { get; set; }
        }

        private enum TestStatus
        {
            Runing,
            Completed,
            CompletedFault,
        }

        private class TestResult
        {
            public TestStatus Status { get; set; }
        }

        #endregion

        #region Data

        private readonly IHost _host;

        private readonly ILogger<IndexingTest> _logger;

        #endregion

        #region .ctor

        public IndexingTest()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile(Path.Combine("Config", "settings.json"));
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(
                        serviceProvider =>
                        {
                            var configuration = serviceProvider
                                .GetRequiredService<IConfiguration>();

                            return configuration.Get<AppSettings>();
                        });
                    services.AddHttpClient(
                        name: "indexing.integration.tests.api",
                        configureClient: (serviceProvider, httpClient) =>
                        {
                            var configuration = serviceProvider
                                .GetRequiredService<AppSettings>();
                            var indexingSettings = configuration.IndexingIntegrationTestsApiSettings;

                            var address = $"{indexingSettings.Host}:{indexingSettings.Port}";

                            httpClient.BaseAddress = new Uri(address);
                        });
                })
                .Build();

            _logger = _host.Services
                .GetRequiredService<ILogger<IndexingTest>>();
        }

        #endregion

        #region IndexingTestAsync

        [Fact]
        public async Task IndexingTestAsync()
        {
            //Arrange
            var client = CreateClient();

            _logger.LogInformation($"{nameof(IndexingTestAsync)} is running.");

            //Act

            _logger.LogInformation($"Start of integration tests...");

            var jobId = await StartIntegrationTestsAsync(client);

            _logger.LogInformation($"Integration tests are running with the JobId ({jobId})");

            _logger.LogInformation($"Waiting for the successful completion of all tests...");

            var (isAllComplete, testsResult) = await CheckIsAllTestsCompleteAsync(
                    jobId: jobId,
                    httpClient: client);

            _logger.LogInformation($"Waiting for the successful completion of all tests is completed.");

            WriteResult(testsResult);

            //Assert
            Assert.True(isAllComplete, $"Not all integration tests were completed successfully or exceeds the timeout");

            _logger.LogInformation($"{nameof(IndexingTestAsync)} is completed.");
        }

        #endregion

        #region Private Methods

        private async Task<Guid> StartIntegrationTestsAsync(
            HttpClient httpClient)
        {
            var (Response, Result) = await httpClient
                .PostResultFromAsync<PostExecuteTestsResponse>(
                    requestUri: IndexingIntegrationTestsUri.Execute);

            return Result.JobId;
        }

        private async Task<(bool IsAllComplete, IReadOnlyCollection<TestResult> TestsResult)> CheckIsAllTestsCompleteAsync(
            Guid jobId,
            HttpClient httpClient)
        {
            if(jobId == default)
            {
                throw new ArgumentException("JobId can not be empty");
            }

            var result = default(bool?);
            var testsResult = default(IReadOnlyCollection<TestResult>);
            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(30));
            var request = new GetTestsResultRequest
            {
                JobId = jobId
            };

            while(!cancellationTokenSource.IsCancellationRequested && result == null)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));

                var (Response, Result) = await httpClient
                    .PostResultAndSerializeContentFromAsync<GetTestsResultResponse, GetTestsResultRequest>(
                        requestUri: IndexingIntegrationTestsUri.Result,
                        content: request);

                testsResult = Result.Result;

                if(Result.Result.All(x => x.Status != TestStatus.Runing))
                {
                    if(Result.Result.All(x => x.Status == TestStatus.Completed))
                    {
                        result = true;
                    }
                    else if(Result.Result.Any(x => x.Status == TestStatus.CompletedFault))
                    {
                        result = false;
                    }
                }
            }

            return (result.Value, testsResult);
        }

        private void WriteResult(IReadOnlyCollection<TestResult> testsResult)
        {
            if(testsResult != default)
            {
                var failed = testsResult
                    .Where(x => x.Status == TestStatus.CompletedFault)
                    .Count();
                var passed = testsResult
                    .Where(x => x.Status == TestStatus.Completed)
                    .Count();
                var total = testsResult.Count;

                _logger.LogInformation($"Failed: {failed}, Passed: {passed}, Total: {total}");
            }
        }

        private HttpClient CreateClient()
        {
            return _host.Services
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient("indexing.integration.tests.api");
        }

        #endregion
    }
}
