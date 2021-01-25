using System;
using System.Linq;
using System.IO;

namespace BattlegroundsMatchData
{
    public class CsvConnector
    {
        private static Config _config;

        internal static string AddQuotes(string str)
        {
            return "\"" + str + "\"";
        }
        public static void Initialize(Config config)
        {
            _config = config;

        }

        public static void WriteGameRecord(GameRecord record)
        {
            if (!File.Exists(_config.CsvGameRecordLocation))
            {
                using (StreamWriter sw = File.CreateText(_config.CsvGameRecordLocation))
                {
                    sw.WriteLine(String.Join(",", record.Headers));
                }
            }

            string output = String.Join(",", record.ToList().Select(x => AddQuotes(x.ToString())));

            using (StreamWriter sw = File.AppendText(_config.CsvGameRecordLocation))
            {
                sw.WriteLine(output);
            }
        }

        public static void WriteBoard(GameRecord record)
        {
            if (!File.Exists(_config.CsvBoardRecordLocation))
            {
                using (StreamWriter sw = File.CreateText(_config.CsvBoardRecordLocation))
                {
                    sw.WriteLine(String.Join(",", record.Snapshot.Headers()));
                }
            }

            TurnSnapshot snap1 = record.Histories.Last();
            TurnSnapshot snap2 = record.Histories[record.Histories.Count - 2];

            string output1 = String.Join(",", snap1.ToList(true).Select(x => AddQuotes(x.ToString())));
            string output2 = String.Join(",", snap2.ToList(true).Select(x => AddQuotes(x.ToString())));

            using (StreamWriter sw = File.AppendText(_config.CsvBoardRecordLocation))
            {
                sw.WriteLine(output1);
                sw.WriteLine(output2);
            }
        }
    }
}
