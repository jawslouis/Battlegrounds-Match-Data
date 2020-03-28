﻿using System;
using System.Collections.Generic;
using System.IO;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace BattlegroundsMatchData
{
    public class BgMatchSpreadsheetConnector
    {
        private static string[] _scopes = { SheetsService.Scope.Spreadsheets }; // Change this if you're accessing Drive or Docs
        private static string _applicationName = "Battlegrounds Match Data";
        private static SheetsService _sheetsService;
        private static Config _config;

        public static void ConnectToGoogle(Config config)
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

            if (_config.TestUpload)
            {
                String range = _config.SheetForMyEndingBoard + "!A1:K";
                UpdateSingleRow(new List<Object>() { "test" }, range);
            }
        }


        public static string UpdateSingleRow(List<Object> data, String range)
        {
            IList<IList<Object>> values = new List<IList<Object>>
            {
                data
            };

            return UpdateData(values, range);
        }

        // Pass in your data as a list of a list (2-D lists are equivalent to the 2-D spreadsheet structure)
        public static string UpdateData(IList<IList<Object>> values, String range)
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
