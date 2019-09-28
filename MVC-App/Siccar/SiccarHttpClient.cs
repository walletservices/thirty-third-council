﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_App.Models;
using Newtonsoft.Json;
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
            while (response.StatusCode == System.Net.HttpStatusCode.Accepted)
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

        public async Task<string> Post(string url, string idToken, string content, string token = null)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);
            if (token != null)
            {
                _client.DefaultRequestHeaders.Add("X-Siccar-Authorization", token);
            }
            var response = await _client.PostAsync(new Uri(url), new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> ExtendTokenAttestation(string url, string idToken, string attestations)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

            var config = new Dictionary<string, string>();
            config.Add("client_id", "thirty-third-council");
            config.Add("grant_type", "wallettoattestations");
            config.Add("token", idToken);
            config.Add("scopes", attestations);
            var httpContent = new FormUrlEncodedContent(config);

            var response = await _client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            var dynamicResponse = JsonConvert.DeserializeObject<JsonResponse>(await response.Content.ReadAsStringAsync());
            return dynamicResponse.access_token;
        }

        public async Task<string> ExtendTokenClaims(string url, string idToken, string claims)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);
            var config = new Dictionary<string, string>();
            config.Add("client_id", "thirty-third-council");
            config.Add("grant_type", "wallettoclaims");
            config.Add("token", idToken);
            config.Add("scopes", claims);
            var httpContent = new FormUrlEncodedContent(config);

            var response = await _client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            var dynamicResponse = JsonConvert.DeserializeObject<JsonResponse>(await response.Content.ReadAsStringAsync());
            return dynamicResponse.access_token;
        }

    }
}
