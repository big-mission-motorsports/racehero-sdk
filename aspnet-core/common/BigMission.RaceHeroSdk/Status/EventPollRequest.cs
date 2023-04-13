using BigMission.RaceHeroSdk.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk.Status
{
    public class EventPollRequest
    {
        public string EventId { get; }

        private ILogger Logger { get; }
        private IRaceHeroClient RhClient { get; }

        /// <summary>
        /// Latest Car status
        /// </summary>
        private readonly Dictionary<string, Racer> racerStatus = new();
        private Event lastEvent;
        private EventStates state;

        public EventPollRequest() { }
        public EventPollRequest(string rhEventId, ILogger logger, IRaceHeroClient raceHeroClient)
        {
            EventId = rhEventId;
            Logger = logger;
            RhClient = raceHeroClient;
        }


        public virtual async Task<(EventStates state, Event evt, Leaderboard leaderboard)> PollEventAsync()
        {
            var sw = Stopwatch.StartNew();
            Event evt = null;
            Leaderboard leaderboard = null;

            if (state == EventStates.WaitingForStart)
            {
                evt = await CheckWaitForStartAsync();
            }
            else if (state == EventStates.Started)
            {
                leaderboard = await PollLeaderboardAsync();
            }

            Logger.Debug($"Updated event in {sw.ElapsedMilliseconds}ms state={state}");

            return (state, evt, leaderboard);
        }

        /// <summary>
        /// Checks on status of the event waiting for it to start.
        /// </summary>
        private async Task<Event> CheckWaitForStartAsync()
        {
            try
            {
                var eventId = EventId;
                Logger.Debug($"Checking for event {eventId} to start");
                var evt = await RhClient.GetEvent(eventId);

                lastEvent = evt;
                var isLive = lastEvent.IsLive;
                var isEnded = false;

                // When the event starts, transition to poll for leaderboard data
                if (isLive)
                {
                    Logger.Info($"Event {eventId} is live, starting to poll for race status");
                    state = EventStates.Started;
                }
                // Check for ended
                else if (isEnded)
                {
                    Logger.Info($"Event {eventId} has ended, terminating subscription polling, waiting for event to restart.");
                    state = EventStates.WaitingForStart;
                }

                return evt;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error polling event");
            }
            return null;
        }


        private async Task<Leaderboard> PollLeaderboardAsync()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                Logger.Trace($"Polling leaderboard for event {EventId}");
                var leaderboard = await RhClient.GetLeaderboard(EventId);

                // Stop polling when the event is over
                if (leaderboard == null || leaderboard.Racers == null)
                {
                    Logger.Info($"Event {EventId} has ended");
                    state = EventStates.WaitingForStart;
                }
                else // Process lap updates
                {
                    var cf = leaderboard.CurrentFlag;
                    var flag = RaceHeroClient.ParseFlag(cf);

                    var logs = new List<Racer>();
                    foreach (var newRacer in leaderboard.Racers)
                    {
                        if (racerStatus.TryGetValue(newRacer.RacerNumber, out var racer))
                        {
                            // Process changes
                            if (racer.CurrentLap != newRacer.CurrentLap)
                            {
                                // Log each new lap
                                logs.Add(newRacer);
                            }
                        }
                        racerStatus[newRacer.RacerNumber] = newRacer;
                    }

                    var latestStatusCopy = racerStatus.Values.ToArray();
                    Logger.Trace($"Processing subscriber car lap changes");
                }

                return leaderboard;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error polling leaderboard");
            }
            return null;
        }
    }
}
