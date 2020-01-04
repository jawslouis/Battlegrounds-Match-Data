using System;
using System.Linq;
using System.IO;

using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace BattlegroundsMatchData
{
    public class BgMatchDataRecord
    {
        public int[] TavernTierTimings = new int[5];
        public string Minions;
        public string Hero;
        public int Rating;
        public int Position;
        public string DateTime;

        internal string AddQuotes(string str)
        {
            return "\"" + str + "\"";
        }

        public string Headers()
        {
            return "Date & Time,Hero,Position,MMR,Ending Minions," + AddQuotes("Turns taken to reach tavern tiers 2,3,4,5,6");
        }

        public override string ToString()
        {
            string tavern = TavernTierTimings.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b);
            return $"{DateTime},{AddQuotes(Hero)},{Position},{Rating},{AddQuotes(Minions)},{tavern}";
        }
    }

    public class BgMatchData
    {
        private static bool _checkRating = false;
        private static int _rating;
        private static int _currentTavernTier;
        private static BgMatchDataRecord _record;
        public static string CsvLocation;

        internal static bool InBgMode(string currentMethod)
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)
            {
                Log.Info($"{currentMethod} - Not in Battlegrounds Mode.");
                return false;
            }
            return true;
        }

        internal static void TurnStart(ActivePlayer player)
        {
            if (!InBgMode("Turn Start")) return;
            string playerString = player == ActivePlayer.Player ? "Player" : "Opponent";
            int turn = Core.Game.GetTurnNumber();
            int level = Core.Game.PlayerEntity.GetTag(GameTag.PLAYER_TECH_LEVEL);

            if (_currentTavernTier != level)
            {
                _record.TavernTierTimings[_currentTavernTier - 1] = turn;
                _currentTavernTier = level;
            }

            Log.Info($"{playerString} - turn {turn} - tavern tier {level}");

            if (player != ActivePlayer.Player)
            {
                TakeBoardSnapshot();
            }
        }

        internal static void TakeBoardSnapshot()
        // take snapshot of current minions board state
        {
            int playerId = Core.Game.Player.Id;

            var entities = Core.Game.Entities.Values
                    .Where(x => x.IsMinion && x.IsInPlay && x.IsControlledBy(playerId))
                    .Select(x => x.LocalizedName)
                    .ToArray();

            _record.Minions = entities.Aggregate((a, b) => a + ", " + b);

            Log.Info("Current minions in play: " + _record.Minions);

        }

        internal static void GameStart()
        {
            if (!InBgMode("Game Start")) return;
            Log.Info("Starting game");
            _record = new BgMatchDataRecord();
        }

        internal static void OnLoad()
        {
            Log.Info($"Loaded Plugin. CSV Location: {CsvLocation}");
            _currentTavernTier = 1;
        }

        internal static void GameEnd()
        {
            if (!InBgMode("Game End")) return;
            int playerId = Core.Game.Player.Id;
            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                .First();
            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);
            _record.Hero = hero.LocalizedName;
            _record.DateTime = DateTime.Now.ToString("yyyy-MM-dd HHmm");

            Log.Info($"Game ended - {_record.Hero} - Position: {_record.Position}");
        }

        internal static void InMenu()
        {
            if (!InBgMode("In Menu")) return;

            _checkRating = true;
            _rating = Core.Game.BattlegroundsRatingInfo.Rating;
            Log.Info($"In Menu - Prev Rating: {_rating}");
        }

        internal static void Update()
        {
            if (_checkRating)
            {
                int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;

                if (_rating != latestRating)
                {
                    if (!InBgMode("Update")) return;

                    _rating = latestRating;
                    _checkRating = false;
                    _record.Rating = _rating;
                    Log.Info($"Rating Updated: {_rating}");

                    if (!File.Exists(CsvLocation))
                    {
                        using (StreamWriter sw = File.CreateText(CsvLocation))
                        {
                            sw.WriteLine(_record.Headers());
                        }
                    }

                    using (StreamWriter sw = File.AppendText(CsvLocation))
                    {
                        sw.WriteLine(_record.ToString());
                    }
                }
            }
        }
    }

}
