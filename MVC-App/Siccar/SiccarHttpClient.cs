using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_App.Siccar
{
    public class SiccarHttpClient : ISiccarHttpClient
    {
        public HttpClient _client;

        public string Get(string url, string idToken, bool ensureResponseIsValid = true)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

            var response = _client.GetAsync(new Uri(url)).GetAwaiter().GetResult();
            if (ensureResponseIsValid)
            {
                response.EnsureSuccessStatusCode();
            }
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
