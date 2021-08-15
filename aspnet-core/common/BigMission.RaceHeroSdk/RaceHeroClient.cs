using BigMission.RaceHeroSdk.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk
{
    /// <summary>
    /// This client wraps access to Race Hero API resources.
    /// </summary>
    public class RaceHeroClient : IRaceHeroClient
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

        public enum Flag { Unknown, Green, Yellow, Red, Warmup, Finish, Stop }
        public static Flag ParseFlag(string flag)
        {
            if (string.IsNullOrWhiteSpace(flag))
            {
                return Flag.Unknown;
            }

            var s = flag.Trim().ToUpper();
            if (s == "GREEN")
            {
                return Flag.Green;
            }
            if (s == "YELLOW")
            {
                return Flag.Yellow;
            }
            if (s == "RED")
            {
                return Flag.Red;
            }
            if (s == "FINISH")
            {
                return Flag.Finish;
            }
            if (s == "STOP")
            {
                return Flag.Stop;
            }
            return Flag.Unknown;
        }
    }
}
