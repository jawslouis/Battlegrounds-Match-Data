using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;


namespace BattlegroundsMatchData
{
    public class SpreadsheetConnector
    {
        private static string[] _scopes = { SheetsService.Scope.Spreadsheets }; // Change this if you're accessing Drive or Docs
        private static string _applicationName = "Battlegrounds Match Data";
        private static SheetsService _sheetsService;
        private static Config _config;

        public static void Initialize(Config config)
        {
            _config = config;

            Log.Info("Connecting to Google");

            GoogleCredential credential;

            // Put your credentials json file in the root of the solution and make sure copy to output dir property is set to always copy 
            using (var stream = new FileStream(config.CredentialLocation,
                FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            // Create Google Sheets API service.
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });

        }

        public static void WriteGameRecord(GameRecord record)
        {
            String range = _config.SheetForMyEndingBoard + "!A1:K";

            IList<IList<Object>> values = new List<IList<Object>> {
                record.ToList()
            };

            UpdateData(values, range);
        }

        public static void WriteBoard(GameRecord record)
        {
            TurnSnapshot snap1 = record.Histories.Last();
            TurnSnapshot snap2 = record.Histories[record.Histories.Count - 2];

            IList<IList<Object>> values = new List<IList<Object>> {
                snap1.ToList(true),
                snap2.ToList(true)
            };

            string range = _config.SheetForAllBoards + "!A1:F";

            UpdateData(values, range);
        }

        // Pass in your data as a list of a list (2-D lists are equivalent to the 2-D spreadsheet structure)
        private static string UpdateData(IList<IList<Object>> values, String range)
        {
            Log.Info("Updating spreadsheet data");


            ValueRange body = new ValueRange
            {
                Values = values
            };

            AppendRequest request = _sheetsService.Spreadsheets.Values.Append(body, _config.SpreadsheetId, range);
            request.ValueInputOption = AppendRequest.ValueInputOptionEnum.RAW;
            AppendValuesResponse response = request.Execute();

            return JsonConvert.SerializeObject(response);
        }
    }
}
