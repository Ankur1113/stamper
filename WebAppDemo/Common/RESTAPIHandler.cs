using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAppDemo.Common
{
    public class RESTAPIHandler : IRESTAPIHandler
    {
        private readonly HttpClient httpClient;
        public RESTAPIHandler(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("httpClient");
        }

        public async Task<object> PostAsync(string endpoint, StringContent content)
        {
            return await httpClient.PostAsync(endpoint, content);
        }

        public async Task<object> PostAsync(string endpoint,MultipartFormDataContent content)
        {
            return await httpClient.PostAsync(endpoint, content);
        }

        public async Task<object> GetAsync(string endpoint)
        {
            return await httpClient.GetAsync(endpoint);
        }

    }
}
