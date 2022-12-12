using FilesManager.Integration.Test.Models;
using FilesManager.Models.ApiModels;
using FilesManagerTests.TestProperties;
using Http.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using System.IO;
using Xunit;
using FilesManagerTests;
using System;

namespace FilesManagerTests
{
    public class TestsApi
    {
        private readonly IHost _host;
        private ConfigSettings _configSettings;
        private string _filesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
        private string _basePath = string.Empty;

        public TestsApi()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile(Path.Combine("EnvironmentConfigs", "config.json"));
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton(
                        serviceProvider =>
                        {
                            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                            return configuration.Get<ConfigSettings>();
                        });
                })
                .Build();
            _configSettings = _host.Services.GetRequiredService<ConfigSettings>();
            _basePath = $"{_configSettings.DevelopmentSettings.Host}:{_configSettings.DevelopmentSettings.Port}";
        }

        [Fact]
        public void ApiDownloadTest()
        {
            string url = $"{_basePath}{ApiEndpoints.DownloadUrl}";
            DownloadRequest downloadRequest = GenerateDownloadRequest(false, true, false);
            Response<DownloadResponse> result = new HttpMaster().PUT(url, downloadRequest).Deserialize<Response<DownloadResponse>>();
            Assert.Equal(0, result.Code);
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(!string.IsNullOrEmpty(result.Data.FileName));
            Assert.True(!string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public void ApiDownloadReadonlyTest()
        {
            string url = $"{_basePath}{ApiEndpoints.DownloadUrl}";
            DownloadRequest downloadRequest = GenerateDownloadRequest(true, true, false);
            Response<DownloadResponse> result = new HttpMaster().PUT(url, downloadRequest).Deserialize<Response<DownloadResponse>>();
            Assert.Equal(0, result.Code);
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(!string.IsNullOrEmpty(result.Data.FileName));
            Assert.True(!string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public void ApiDownloadNotTrackingTest()
        {
            string url = $"{_basePath}{ApiEndpoints.DownloadUrl}";
            DownloadRequest downloadRequest = GenerateDownloadRequest(false, true, false);
            Response<DownloadResponse> result = new HttpMaster().PUT(url, downloadRequest).Deserialize<Response<DownloadResponse>>();
            Assert.Equal(0, result.Code);
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.True(!string.IsNullOrEmpty(result.Data.FileName));
            Assert.True(!string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public void ApiConectTest()
        {
            string url = $"{_basePath}{ApiEndpoints.ConnectUrl}={_configSettings.DevelopmentSettings.Version}";
            Response<ConnectResponse> result = new HttpMaster()
                .GET(url).Deserialize<Response<ConnectResponse>>();
            Assert.Equal(0, result.Code);
            Assert.NotNull(result);
            Assert.Equal(FilesManager.Constants.Platforms.Windows, result.Data.Platform);

        }

        private DownloadRequest GenerateDownloadRequest(bool readonlyflag, bool trackHistory, bool openForView)
        {
            DownloadRequest downloadRequest = new();
            int identifier = GetRandomInt(10000);
            string wordFilePath = Path.Combine(_filesDirectory, "test.docx");
            byte[] bytes = File.ReadAllBytes(wordFilePath);
            string fileBase64 = Convert.ToBase64String(bytes);
            downloadRequest.FileBase64 = fileBase64;
            downloadRequest.FileName = $"test{identifier}.docx";
            downloadRequest.FileVersionId = identifier;
            downloadRequest.FileId = identifier;
            downloadRequest.Readonly = readonlyflag;
            downloadRequest.TrackHistory = trackHistory;
            downloadRequest.OpenForView = openForView;
            downloadRequest.UID = $"signalR_{identifier}";
            return downloadRequest;
        }


        private int GetRandomInt(int maxValue)
        {
            Random rnd = new Random();
            return rnd.Next(0, maxValue);
        }
    }
}
