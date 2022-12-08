using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LanDocs.Indexing.IntegrationTests.Extensions
{
    public static class HttpClientExtensions
    {
        #region Public Methods

        public static Task<HttpResponseMessage> PostResultFromAsync(
            this HttpClient client,
            HttpRequestMessage requestMessage)
        {
            return client.SendAsync(requestMessage);
        }

        public static Task<HttpResponseMessage> PostResultFromAsync<TContent>(
            this HttpClient client,
            Uri requestUri,
            TContent content) where TContent : HttpContent
        {
            return client.PostAsync(requestUri, content);
        }

        public static async Task<(HttpResponseMessage Response, TResult Result)> PostResultFromAsync<TResult>(
            this HttpClient client,
            HttpRequestMessage requestMessage)
        {
            var response = await client.SendAsync(requestMessage);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {requestMessage.RequestUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await GetResultFromAsync<TResult>(response);

            return (response, result);
        }

        public static async Task<(HttpResponseMessage Response, TResult Result)> PostResultFromAsync<TResult, TContent>(
            this HttpClient client,
            Uri requestUri,
            TContent content) where TContent : HttpContent
        {
            var response = await client.PostAsync(requestUri, content);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {requestUri.AbsoluteUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await GetResultFromAsync<TResult>(response);

            return (response, result);
        }

        public static async Task<(HttpResponseMessage Response, TResult Result)> PostResultFromAsync<TResult>(
            this HttpClient client,
            string requestUri)
        {
            var response = await client.PostAsync(requestUri, default);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {client.BaseAddress}{requestUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await GetResultFromAsync<TResult>(response);

            return (response, result);
        }

        public static Task<HttpResponseMessage> PostResultAndSerializeContentFromAsync<TContent>(
            this HttpClient client,
            Uri requestUri,
            TContent content)
        {
            var contentJson = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            return client.PostAsync(requestUri, stringContent);
        }

        public static async Task<(HttpResponseMessage Response, TResult Result)> PostResultAndSerializeContentFromAsync<TResult, TContent>(
            this HttpClient client,
            Uri requestUri,
            TContent content)
        {
            var contentJson = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, stringContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {requestUri.AbsoluteUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await GetResultFromAsync<TResult>(response);

            return (response, result);
        }

        public static async Task<(HttpResponseMessage Response, TResult Result)> PostResultAndSerializeContentFromAsync<TResult, TContent>(
            this HttpClient client,
            string requestUri,
            TContent content)
        {
            var contentJson = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, stringContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {client.BaseAddress}/{requestUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await GetResultFromAsync<TResult>(response);

            return (response, result);
        }

        public static async Task<(HttpResponseMessage Response, string Result)> PostStringResultAndSerializeContentFromAsync<TContent>(
            this HttpClient client,
            Uri requestUri,
            TContent content)
        {
            var contentJson = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(contentJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUri, stringContent);

            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Request on {requestUri.AbsoluteUri} return doesn't success StatusCode: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();

            return (response, result);
        }

        #endregion

        #region Private Methods

        private static async Task<T> GetResultFromAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(resultStream, options);

            return result;
        }

        #endregion
    }
}
