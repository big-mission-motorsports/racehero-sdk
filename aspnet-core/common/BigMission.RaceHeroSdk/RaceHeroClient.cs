using BigMission.RaceHeroSdk.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk
{
    /// <summary>
    /// This client wraps access to Race Hero API resources.
    /// </summary>
    public class RaceHeroClient
    {
        private string RootUrl { get; }
        private string ApiKey { get; }

        public RaceHeroClient(string rootUrl, string apiKey)
        {
            RootUrl = rootUrl;
            ApiKey = apiKey;
        }

        public async Task<Events> GetEvents(int limit = 25, int offset = 0, bool live = false)
        {
            var client = new RestClient(RootUrl);
            client.Authenticator = new HttpBasicAuthenticator(ApiKey, "");
            var request = new RestRequest($"events?limit={limit}&offset={offset}&live={live.ToString().ToLower()}&expand=", DataFormat.Json);
            return await client.GetAsync<Events>(request);
        }

        public async Task<Event> GetEvent(string eventId)
        {
            var client = new RestClient(RootUrl);
            client.Authenticator = new HttpBasicAuthenticator(ApiKey, "");
            var request = new RestRequest($"events/{eventId}?expand=", DataFormat.Json);
            return await client.GetAsync<Event>(request);
        }

        public async Task<Leaderboard> GetLeaderboard(string eventId)
        {
            var client = new RestClient(RootUrl);
            client.Authenticator = new HttpBasicAuthenticator(ApiKey, "");
            var request = new RestRequest($"events/{eventId}/live/leaderboard", DataFormat.Json);
            return await client.GetAsync<Leaderboard>(request);
        }

    }
}
