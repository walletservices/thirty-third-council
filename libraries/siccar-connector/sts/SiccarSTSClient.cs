using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Siccar.Connector.STS
{
    public class SiccarSTSClient : ISiccarSTSClient
    {
        private string _clientId;
        private string _attestationsGrantType = "wallettoattestations";
        private string _claimsGrantType = "wallettoclaims";

        public HttpClient _client;


        public SiccarSTSClient(string clientId, HttpClient client)
        {
            //config.Add("client_id", "thirty-third-council");
            _clientId = clientId;
            _client = client;
        }

        public async Task<string> ExtendTokenAttestation(string url, string idToken, string attestations)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);

            var config = new Dictionary<string, string>();
            config.Add("client_id", _clientId);
            config.Add("grant_type", _attestationsGrantType);
            config.Add("token", idToken);
            config.Add("scopes", attestations);
            var httpContent = new FormUrlEncodedContent(config);

            var response = await _client.PostAsync(url, httpContent);



            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var dynamicResponse = JsonConvert.DeserializeObject<STSResponse>(stringResponse);
            return dynamicResponse.access_token;
        }

        public async Task<string> ExtendTokenClaims(string url, string idToken, string claims)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);
            var config = new Dictionary<string, string>();
            config.Add("client_id", _clientId);
            config.Add("grant_type", _claimsGrantType);
            config.Add("token", idToken);
            config.Add("scopes", claims);
            var httpContent = new FormUrlEncodedContent(config);

            var response = await _client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            var dynamicResponse = JsonConvert.DeserializeObject<STSResponse>(await response.Content.ReadAsStringAsync());
            return dynamicResponse.access_token;
        }

    }
}
