using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public class SiccarHttpClient : ISiccarHttpClient
    {
        public HttpClient _client;

        public async Task<string> Get(string url, string idToken, bool ensureResponseIsValid = true)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

            var response = await _client.GetAsync(new Uri(url));
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> Poll(string url, string idToken)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);
            var response = await _client.GetAsync(new Uri(url));
            while(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Thread.Sleep(10);
                response = await _client.GetAsync(new Uri(url));
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> Post(string url, string idToken, string content, bool ensureResponseIsValid = true)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

            var response = await _client.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, "application/json"));
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Content.ReadAsStringAsync().Result;

        }
    }
}
