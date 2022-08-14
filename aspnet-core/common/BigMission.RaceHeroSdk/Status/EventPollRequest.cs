using BigMission.RaceHeroSdk.Models;
using BigMission.TestHelpers;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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


        public EventPollRequest(string rhEventId, ILogger logger, IRaceHeroClient raceHeroClient)
        {
            EventId = rhEventId;
            Logger = logger;
            RhClient = raceHeroClient;
            //waitForStartInterval = TimeSpan.FromMilliseconds(int.Parse(Config["WaitForStartTimer"]));
            //eventPollInterval = TimeSpan.FromMilliseconds(int.Parse(Config["EventPollTimer"]));
            //logRHToFile = bool.Parse(Config["LogRHToFile"]);
            //readTestFiles = bool.Parse(Config["ReadTestFiles"]);
        }


        public async Task<(EventStates state, Event evt, Leaderboard leaderboard)> PollEventAsync()
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
                //await LogEventPoll(evt);
                //await PublishEventStatus(currentEventStatus, evt, null);

                lastEvent = evt;
                var isLive = lastEvent.IsLive;
                var isEnded = false;

                // When the event starts, transition to poll for leaderboard data
                if (isLive)
                {
                    //lastFlagChange = DateTime.Now;
                    Logger.Info($"Event {eventId} is live, starting to poll for race status");

                    //if (int.TryParse(EventId, out int eid))
                    //{
                    //    flagStatus = new FlagStatus(eid, Logger, Config, cacheMuxer, DateTime);
                    //}

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
                //await PublishEventStatus(currentEventStatus, null, leaderboard);
                //await LogLeaderboardPoll(EventId, leaderboard);

                // Stop polling when the event is over
                if (leaderboard == null || leaderboard.Racers == null)
                {
                    Logger.Info($"Event {EventId} has ended");
                    //await flagStatus?.EndEvent();
                    state = EventStates.WaitingForStart;
                }
                else // Process lap updates
                {
                    var cf = leaderboard.CurrentFlag;
                    var flag = RaceHeroClient.ParseFlag(cf);

                    //// Simulate yellow flags
                    //if (simulateSettingsService.Settings.YellowFlags)
                    //{
                    //    if ((DateTime.Now - lastFlagChange) > TimeSpan.FromMinutes(1))
                    //    {
                    //        if (overrideFlag == null || overrideFlag == RaceHeroClient.Flag.Green)
                    //        {
                    //            overrideFlag = RaceHeroClient.Flag.Yellow;
                    //            lastFlagChange = DateTime.Now;
                    //        }
                    //        else if (overrideFlag == RaceHeroClient.Flag.Yellow)
                    //        {
                    //            overrideFlag = RaceHeroClient.Flag.Green;
                    //            lastFlagChange = DateTime.Now;
                    //        }
                    //    }
                    //}

                    //await flagStatus?.ProcessFlagStatus(overrideFlag ?? flag, leaderboard.RunId);

                    var logs = new List<Racer>();
                    foreach (var newRacer in leaderboard.Racers)
                    {
                        if (racerStatus.TryGetValue(newRacer.RacerNumber, out var racer))
                        {
                            //// Simulate code for pit stops
                            //if (simulateSettingsService.Settings.PitStops)
                            //{
                            //    if (newRacer.RacerNumber.Contains("777"))
                            //    {
                            //        if ((DateTime.Now - lastPitStop) > TimeSpan.FromMinutes(4))
                            //        {
                            //            lastPitLap = newRacer.CurrentLap;
                            //            lastPitStop = DateTime.Now;
                            //        }
                            //        newRacer.LastPitLap = lastPitLap;
                            //    }
                            //}

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

                    // Update car data with full current field
                    //subscriberCars.ForEach(async c => { await c.ProcessUpdate(latestStatusCopy); });
                    //Logger.Trace($"latestStatusCopy {sw.ElapsedMilliseconds}ms");

                    //if (logs.Any())
                    //{
                    //    var eid = int.Parse(EventId);
                    //    var now = DateTime.UtcNow;
                    //    var carRaceLaps = new List<CarRaceLap>();
                    //    foreach (var l in logs)
                    //    {
                    //        var log = new CarRaceLap
                    //        {
                    //            EventId = eid,
                    //            RunId = leaderboard.RunId,
                    //            CarNumber = l.RacerNumber,
                    //            Timestamp = now,
                    //            CurrentLap = l.CurrentLap,
                    //            ClassName = l.RacerClassName,
                    //            LastLapTimeSeconds = l.LastLapTimeSeconds,
                    //            PositionInRun = l.PositionInRun,
                    //            LastPitLap = l.LastPitLap,
                    //            PitStops = l.PitStops,
                    //            Flag = (byte)flag
                    //        };
                    //        carRaceLaps.Add(log);
                    //    }

                    //    await CacheToFuelStatistics(carRaceLaps);
                    //    Logger.Trace($"CacheToFuelStatistics {sw.ElapsedMilliseconds}ms");

                    //    if (!readTestFiles)
                    //    {
                    //        await LogLapChanges(carRaceLaps);
                    //    }
                    //}
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
