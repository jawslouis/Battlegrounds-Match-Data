using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace BattlegroundsMatchData
{
    public class GraphqlConnector
    {
        private static readonly HttpClient client = new HttpClient();
        private static Config _config;

        public static void Initialize(Config config)
        {
            _config = config;
            //Test();
        }


        public static string ListToString(List<(string, object)> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var (key, val) = list[i];

                if (val is DateTimeOffset) val = val.ToString();

                if (val is string) list[i] = (key, $"\"{val}\"");

            }

            string result = String.Join(", ", list.Select(item => $"{item.Item1}:{item.Item2}"));
            return result;

        }

        public static string TimeZoneOffset()
        {
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).ToString();
        }

        public static void WriteGameRecord(GameRecord record)
        {

            List<(string, object)> recordList = new List<(string, object)>
            {
                ("tavernTimings", String.Join(",", record.PaddedTavernTimings())),
                ("position", record.Position),
                ("endTurn", record.Snapshot.Turn),
                ("mmr", record.Rating),
                ("mmrChange", record.MmrChange),
                ("hero", record.Snapshot.Hero),
                ("gameId", record.Snapshot.GameID),
                ("dateTime", record.DateTime),
                ("player", record.player),
            };
            string recordString = ListToString(recordList);

            string queryString = $"mutation {{createGameRecord({recordString}) {{ ok }} }}";
            PostGraphql(queryString);
        }

        public static void WriteBoard(GameRecord record)
        {
            TurnSnapshot snap1 = record.Histories.Last();
            TurnSnapshot snap2 = record.Histories[record.Histories.Count - 2];

            string board1 = ListToString(snap1.ToArgList());
            string board2 = ListToString(snap2.ToArgList());

            string queryString = $"mutation {{board1:createBoard({board1}) {{ ok }} board2:createBoard({board2}) {{ ok }} }} ";
            PostGraphql(queryString);
        }

        public static async void PostGraphql(string queryString)
        {
            var values = new Dictionary<string, string>{
                { "Content-Type", "application/json" },
                { "query", queryString}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://bgstats.cintrest.com/graphql", content);
            //var response = await client.PostAsync("http://localhost:8000/bg_stats/graphql", content);

            var responseString = await response.Content.ReadAsStringAsync();

            //MessageBox.Show(queryString);
            //MessageBox.Show(responseString);
        }
    }
}
