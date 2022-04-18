using BigMission.RaceHeroSdk.Models;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk
{
    public interface IRaceHeroClient
    {
        Task<Events> GetEvents(int limit = 25, int offset = 0, bool live = false);
        Task<Event> GetEvent(string eventId);
        Task<Leaderboard> GetLeaderboard(string eventId);
        Task<LiveTransponder> GetLiveTransponder(string transponderId);
    }
}
