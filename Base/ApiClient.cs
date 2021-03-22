using NetAPorter.Logging;
using NetAPorter.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace NetAPorter.Base
{
    public class ApiClient
    {
        private IHttpClientFactory _clientFactory;
        private readonly ScenarioContext _scenarioContext;
        private static readonly ILogger Logger = LoggerFactory.CreateLogger();
        private AppSettings _appSettings;
        private const string JwtDefaultContentType = "application/jwt";
        private const string JsonDefaultContentType = "application/json";
        private static StringContent _requestContent;
        private static HttpContent _encodedContentRequest;
        private string _errorResponseMessage;

        public ApiClient(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public async Task<T> GetAsync<T>(string endpoint) where T : class
        {
            _appSettings = _scenarioContext.Get<AppSettings>();
            _clientFactory = _scenarioContext.Get<IHttpClientFactory>();
            var client = _clientFactory.CreateClient();

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(true);
                var res = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) throw new Exception($"Failed on: {endpoint} Endpoint, and Response body returned was: {res}");
                //response.ResponseHeadersAssertion<T>(_appSettings, _scenarioContext);


                if (typeof(string).IsAssignableFrom(typeof(T)))
                {
                    return res as T;
                }

                return JsonConvert.DeserializeObject<T>(res);
            }
            catch (Exception exception) when (exception is HttpRequestException ||
                                              exception is SocketException)
            {
                Logger.Write($"Get request failed on this endpoint : {endpoint}, and Inner exception was: ", exception.InnerException, EventSeverity.Error);
                throw;
            }
        }
        public async Task<T> TestharnessPostAsync<T>(string endpoint, object requestObject) where T : class
        {
            _appSettings = _scenarioContext.Get<AppSettings>();
            _clientFactory = _scenarioContext.Get<IHttpClientFactory>();
            var client = _clientFactory.CreateClient("testharness");

            var content = requestObject is string
                ? requestObject.ToString()
                : await Task.Factory.StartNew(() => JsonConvert.SerializeObject(requestObject, Formatting.Indented, JsonSerializerSettings));
            var requestContent = new StringContent(content, Encoding.UTF8, JsonDefaultContentType);

            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = requestContent
                };
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                var res = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode) throw new Exception($"Failed on: {endpoint} Endpoint, and Response body returned was: {res}");

                //response.ResponseHeadersAssertion<T>(_cookieContainer, _appSettings, _scenarioContext);
                return JsonConvert.DeserializeObject<T>(res);
            }
            catch (Exception exception) when (exception is WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                Logger.Write($"Get request failed on this endpoint : {endpoint}, and Inner exception was: ", exception.InnerException, EventSeverity.Error);
                throw;
            }
        }

    }
}
