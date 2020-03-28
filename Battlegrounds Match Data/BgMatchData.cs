using System;
using System.Linq;
using System.IO;

using HearthDb.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using Hearthstone_Deck_Tracker.Utility.Logging;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker.Utility;

namespace BattlegroundsMatchData
{

    public class BgMatchDataSnapshot
    {
        public string Minions;
        public string Hero;
        public int Turn;
        public string dateTime;
        public int Position;
        public string isSelf = "Yes";
    }


    public class BgMatchDataRecord
    {
        public List<int> TavernTierTimings = new List<int>();
        public int CurrentTavernTier = 1;
        public BgMatchDataSnapshot Snapshot = new BgMatchDataSnapshot();
        public List<BgMatchDataSnapshot> Histories = new List<BgMatchDataSnapshot>();
        public int Rating;

        public string DateTime { get => Snapshot.dateTime; set => Snapshot.dateTime = value; }
        public int Position { get => Snapshot.Position; set => Snapshot.Position = value; }

        internal string AddQuotes(string str)
        {
            return "\"" + str + "\"";
        }

        public string Headers()
        {
            return "Date & Time,Hero,Position,MMR,Ending Minions," + AddQuotes("Turns taken to reach tavern tiers 2,3,4,5,6") + ",,,,,Ending Turn";
        }



        public override string ToString()
        {
            string tavern;
            if (TavernTierTimings.Count > 0)
                tavern = TavernTierTimings.Select(x => x.ToString()).Aggregate((a, b) => a + "," + b);
            else tavern = "";

            return $"{DateTime},{AddQuotes(Snapshot.Hero)},{Position},{Rating},{AddQuotes(Snapshot.Minions)},{tavern},{Snapshot.Turn}";
        }

        public List<object> ToList()
        {
            List<object> l = new List<object>
            {
                DateTime, Snapshot.Hero, Position, Rating, Snapshot.Minions
            };

            foreach (int turn in TavernTierTimings)
            {
                l.Add(turn);
            }

            for (int i = 0; i < 5 - TavernTierTimings.Count; i++)
            {
                l.Add("");
            }

            l.Add(Snapshot.Turn);

            return l;
        }

        public IList<IList<Object>> HistoryToList()
        {
            IList<IList<Object>> values = new List<IList<Object>>();

            foreach (BgMatchDataSnapshot s in Histories)
            {
                List<object> l = new List<object>
                {
                    s.dateTime, s.Hero, s.Minions, s.Turn, s.isSelf
                };
                values.Add(l);
            }

            return values;
        }
    }

    public class BgMatchData
    {
        private static bool _checkRating = false;
        private static int _checkStats = 0;
        private static int _rating;
        private static BgMatchDataRecord _record;
        private static Config _config;

        public static BgMatchOverlay Overlay;

        private static Dictionary<GameTag, string> RelevantTags = new Dictionary<GameTag, string>()
        {
            [GameTag.TAUNT] = LocUtil.Get("GameTag_Taunt"),
            [GameTag.DIVINE_SHIELD] = LocUtil.Get("GameTag_DivineShield"),
            [GameTag.POISONOUS] = LocUtil.Get("GameTag_Poisonous"),
            [GameTag.WINDFURY] = LocUtil.Get("GameTag_Windfury"),
            [GameTag.DEATHRATTLE] = LocUtil.Get("GameTag_Deathrattle")
        };



        internal static bool InBgMode(string currentMethod)
        {
            if (Core.Game.CurrentGameMode != GameMode.Battlegrounds)
            {
                Log.Info($"{currentMethod} - Not in Battlegrounds Mode.");
                return false;
            }
            return true;
        }

        internal static string MinionToString(Entity entity)
        {
            if (entity == null) return null;

            string attack = entity.GetTag(GameTag.ATK).ToString();
            string health = entity.GetTag(GameTag.HEALTH).ToString();
            string info = $"{attack}/{health}";

            var tags = RelevantTags.Keys.Where(x => entity.HasTag(x)).Select(x => RelevantTags[x]);
            if (tags.Count() > 0)
            {
                info += " " + string.Join(", ", tags);
            }

            string str = $"{entity.LocalizedName} ({info})";

            return str;
        }

        internal static float MinionToAtk(Entity entity)
        {
            return entity.GetTag(GameTag.ATK);
        }

        internal static float MinionToHealth(Entity entity)
        {
            return entity.GetTag(GameTag.HEALTH);
        }


        internal static void PlayerPlay(Card card)
        {
            UpdateStats();
            _checkStats = 3; // check for 300ms after, since battlecries may have triggered
        }

        internal static void TurnStart(ActivePlayer player)
        {
            if (!InBgMode("Turn Start")) return;

            int turn = Core.Game.GetTurnNumber();

            int level = Core.Game.PlayerEntity.GetTag(GameTag.PLAYER_TECH_LEVEL);

            if (_record.CurrentTavernTier != level)
            {
                _record.TavernTierTimings.Add(turn);
                _record.CurrentTavernTier = level;
            }

            // take snapshot of current minions board state
            int playerId = Core.Game.Player.Id;

            BgMatchDataSnapshot Snapshot = CreatePlayerSnapshot(playerId, turn);

            Log.Info("Current minions in play: " + Snapshot.Minions);
            _record.Snapshot = Snapshot;

            UpdateStats();

            bool isOpponentTurn = player == ActivePlayer.Opponent;

            if (turn >= _config.TurnToStartTrackingAllBoards && isOpponentTurn)
            {
                _record.Histories.Add(Snapshot);

                // record opponent's board too
                BgMatchDataSnapshot OppSnapshot = CreatePlayerSnapshot(Core.Game.Opponent.Id, turn);
                OppSnapshot.isSelf = "";
                Log.Info($"Opponent: ({OppSnapshot.Hero}) - Minions in play: {OppSnapshot.Minions}");
                _record.Histories.Add(OppSnapshot);

            }

        }

        private static void UpdateStats()
        {
            int playerId = Core.Game.Player.Id;

            float[] atk = Core.Game.Entities.Values
                    .Where(x => x.IsMinion && x.IsInPlay && x.IsControlledBy(playerId))
                    .Select(x => MinionToAtk(x))
                    .ToArray();

            float[] health = Core.Game.Entities.Values
                    .Where(x => x.IsMinion && x.IsInPlay && x.IsControlledBy(playerId))
                    .Select(x => MinionToHealth(x))
                    .ToArray();

            Overlay.UpdateTotalStats((int)atk.Sum(), (int)health.Sum());
            Overlay.UpdateAvgStats(atk.Average(), health.Average());
        }

        private static BgMatchDataSnapshot CreatePlayerSnapshot(int playerId, int turn)
        {
            BgMatchDataSnapshot Snapshot = new BgMatchDataSnapshot();
            var entities = Core.Game.Entities.Values
                    .Where(x => x.IsMinion && x.IsInPlay && x.IsControlledBy(playerId))
                    .Select(x => MinionToString(x))
                    .ToArray();

            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.IsInPlay && x.IsControlledBy(playerId))
                .FirstOrDefault();

            Snapshot.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

            Snapshot.Hero = hero.LocalizedName;
            Snapshot.Minions = entities.Aggregate((a, b) => a + ", " + b);
            Snapshot.Turn = turn;
            Snapshot.dateTime = DateTime.Now.ToString("yyyy-MM-dd HHmm");

            return Snapshot;
        }

        internal static void GameStart()
        {
            if (!InBgMode("Game Start")) return;
            Log.Info("Starting game");
            _record = new BgMatchDataRecord();
            Overlay.UpdateTotalStats(0, 0);
            Overlay.UpdateAvgStats(0, 0);            
        }

        internal static void OnLoad(Config config)
        {
            _config = config;
            Log.Info($"Loaded Plugin. CSV Location: {config.CsvLocation}");

        }

        internal static void GameEnd()
        {
            if (!InBgMode("Game End")) return;
            int playerId = Core.Game.Player.Id;
            Entity hero = Core.Game.Entities.Values
                .Where(x => x.IsHero && x.GetTag(GameTag.PLAYER_ID) == playerId)
                .First();
            _record.Position = hero.GetTag(GameTag.PLAYER_LEADERBOARD_PLACE);

            Log.Info($"Game ended - {_record.Snapshot.Hero} - Position: {_record.Position}");            
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

            if (_checkStats > 0)
            {
                _checkStats--;
                UpdateStats();
            }

            // rating is only updated after we have passed the menu
            if (_checkRating)
            {
                int latestRating = Core.Game.BattlegroundsRatingInfo.Rating;

                Log.Info($"Checking rating. Current time is: {DateTime.Now.ToString()}, {DateTime.Now.Millisecond.ToString()}ms ");

                if (!InBgMode("Update")) return;

                _rating = latestRating;
                _checkRating = false;
                _record.Rating = _rating;
                Log.Info($"Rating Updated: {_rating}");

                if (!File.Exists(_config.CsvLocation))
                {
                    using (StreamWriter sw = File.CreateText(_config.CsvLocation))
                    {
                        sw.WriteLine(_record.Headers());
                    }
                }

                using (StreamWriter sw = File.AppendText(_config.CsvLocation))
                {
                    sw.WriteLine(_record.ToString());
                }

                if (_config.UploadEnabled)
                {

                    String range = _config.SheetForMyEndingBoard + "!A1:K";
                    BgMatchSpreadsheetConnector.UpdateSingleRow(_record.ToList(), range);

                    range = _config.SheetForAllBoards + "!A1:E";
                    BgMatchSpreadsheetConnector.UpdateData(_record.HistoryToList(), range);

                }

            }
        }
    }

}
