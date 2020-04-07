using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Siccar.Connector.Http
{
    public class SiccarHttpClient : ISiccarHttpClient
    {
        public HttpClient _client;

        public SiccarHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> Get(string url, string idToken, bool ensureResponseIsValid = true)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var response = await _client.GetAsync(new Uri(url));
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<FileContentResult> GetDocument(string url, string idToken, bool ensureResponseIsValid = true)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);

            var response = await _client.GetAsync(new Uri(url));
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }

            return new FileContentResult(response.Content.ReadAsByteArrayAsync().Result, response.Content.Headers.ContentType.MediaType);
        }

        public async Task<string> Poll(string url, string idToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            var response = await _client.GetAsync(new Uri(url));
            while (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Thread.Sleep(10);
                response = await _client.GetAsync(new Uri(url));
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> Post(string url, string idToken, string content, bool ensureResponseIsValid = true)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            var response = await _client.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, "application/json"));
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> Post(string url, string idToken, string content, List<string> tokens = null)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
            if (tokens != null)
            {
                foreach (var token in tokens)
                {
                    _client.DefaultRequestHeaders.Add("X-Siccar-Authorization", token);
                }
            }
            var response = await _client.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }


    }
}
