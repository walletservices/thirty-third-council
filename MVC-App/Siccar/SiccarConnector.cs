using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVC_App
{
    public class SiccarConnector : ISiccarConnector
    {
        public ISiccarOptions Options { get; set; }
        public string Token { get; set; }
        public SiccarConnector(ISiccarOptions options)
        {
            Options = options;
        }

        public void removeUserFromGroup(string userId, string groupId)
        {
            var client = new HttpClient();

            var endpoint = Options.removeUserFromGroup
                .Replace("{groupId}", groupId)
                .Replace("{memberId", userId);

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            var response = client.DeleteAsync(new Uri(endpoint)).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

        }
        public List<KeyValuePair<string, string>> getWalletsUserIsIn(string userId)
        {
            var client = new HttpClient();
            var endpoint = Options.getUsersWallets.Replace("{userId}", userId);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

            var response = client.GetAsync(new Uri(endpoint)).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
            var text = response.Content.ReadAsStringAsync().Result;
            dynamic objs = JsonConvert.DeserializeObject(text);

            var list = new List<KeyValuePair<string, string>>();
            foreach(var o in objs)
            {
                list.Add(new KeyValuePair<string, string>(o.displayName.value, o.id.Value));
            }
            return list;
        }
        public List<KeyValuePair<string, string>> getWallets()
        {

            var client = new HttpClient();
            var addUserEndpoint = Options.getGroups;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            var response = client.GetAsync(new Uri(addUserEndpoint)).GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();

            var text = response.Content.ReadAsStringAsync().Result;

            dynamic objs = JsonConvert.DeserializeObject(text);

            var list = new List<KeyValuePair<string, string>>();
            foreach (var o in objs)
            {
                list.Add(new KeyValuePair<string, string>(o.displayName.Value, o.id.Value));
            }

            return list;
        }
        public List<KeyValuePair<string, string>> getWalletsUserIsNotIn(string userId)
        {
            var walletsUserIsIn = getWalletsUserIsIn(userId);
            var allWallets = getWallets();

            var allWalletIds = allWallets.Select(x => x.Value).ToList<string>();
            var useWalletIds = walletsUserIsIn.Select(x => x.Value).ToList<string>();

            var walletsUserIsNotIn = allWalletIds.Except(useWalletIds);

            return allWallets.Where(x => walletsUserIsNotIn.Contains(x.Value)).ToList<KeyValuePair<string, string>>();

        }
        public bool isUserInProcessDesignersGroup(string userId)
        {
            return getWalletsUserIsIn(userId).Select(x => x.Value).Contains(Options.processDesignersId);
        }
        public void createActor(string userId, string groupName)
        {
            var client = new HttpClient();
            var createActorEndpoint = Options.createActors;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

            if(!groupName.StartsWith("_siccar_actor_wallet"))
            {
                groupName = "_siccar_actor_wallet" + groupName;
            }

            dynamic d = new { groupName = groupName, Userid = userId };
            var jsonString = JsonConvert.SerializeObject(d);

            var response = client.PostAsync(new Uri(createActorEndpoint), new StringContent(jsonString, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();
        }
        public void setToken(string token)
        {
            Token = token;
        }

        public void addUserToGroup(string userId, string groupId)
        {
            var client = new HttpClient();

            var addUserEndpoint = Options.addUserToGroup
                .Replace("{groupId", groupId)
                .Replace("{memberId", userId);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);

            var response = client.PostAsync(new Uri(addUserEndpoint), new StringContent("", Encoding.UTF8, "application/json")).GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();
        }



        public List<KeyValuePair<string, string>> getUsers(string startingLetter)
        {
            var client = new HttpClient();
            var addUserEndpoint = Options.getUsers + startingLetter;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            var response = client.GetAsync(new Uri(addUserEndpoint)).GetAwaiter().GetResult();

            response.EnsureSuccessStatusCode();

            var text = response.Content.ReadAsStringAsync().Result;

            dynamic objs = JsonConvert.DeserializeObject(text);

            var list = new List<KeyValuePair<string, string>>();
            foreach(var o in objs)
            {
                list.Add(new KeyValuePair<string, string>(o.displayName.Value, o.Id.Value));
            }

            return list;
        }


    }
}
