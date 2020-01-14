using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace BattlegroundsMatchData
{
    public class Config
    {
        public static readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BattlegroundsMatchData\BattlegroundsMatchData.config";
        public string CsvLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\BattlegroundsMatchData.csv";
        public bool UploadEnabled = true;
        public bool TestUpload = false;
        public string SheetName = "Sheet1";
        public string SpreadsheetId;
        public string CredentialLocation;

        public void save()
        {
            File.WriteAllText(_configLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

    }

    public class BgMatchDataPlugin : IPlugin
    {
        private Config config;
        private BgMatchOverlay _overlay;

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            GameEvents.OnGameStart.Add(BgMatchData.GameStart);
            GameEvents.OnTurnStart.Add(BgMatchData.TurnStart);
            GameEvents.OnGameEnd.Add(BgMatchData.GameEnd);
            GameEvents.OnInMenu.Add(BgMatchData.InMenu);

            try {
                // load config from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));                
            } catch {
                // create config file
                config = new Config();
                config.save();
            }
      
            BgMatchData.OnLoad(config);

            // connect to Google            
            if (config.UploadEnabled) BgMatchSpreadsheetConnector.ConnectToGoogle(config);

            _overlay = new BgMatchOverlay();
            Core.OverlayCanvas.Children.Add(_overlay);
            _overlay.UpdatePosition();
            BgMatchData.Overlay = _overlay;
        }
    
        public void OnUnload()
        {
            // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
            // see https://git.io/vxEcH

            Core.OverlayCanvas.Children.Remove(_overlay);
        }

        public void OnButtonPress()
        {
            // Triggered when the user clicks your button in the plugin list
            ShowForm();
        }

        public void OnUpdate()
        {
            // called every ~100ms
            BgMatchData.Update();
        }

        public string Name => "Battlegrounds Match Data";

        public string Description => "Save your match statistics in a local CSV file. Tracks the hero, ending position, ending minions, and the turns to reach tavern tiers for each match.";

        public string ButtonText => "Set CSV Location";

        public string Author => "JawsLouis";

        public Version Version => new Version(0, 2, 2);

        public MenuItem MenuItem => CreateMenu();

        private MenuItem CreateMenu()
        {
            MenuItem m = new MenuItem {Header = "Battlegrounds Match Data"};

            MenuItem setDirectory = new MenuItem {Header = "Set CSV Location"};
            setDirectory.Click += (sender, args) =>
            {
                ShowForm();
            };
            m.Items.Add(setDirectory);

            MenuItem enableUpload = new MenuItem {
                Header = "Enable Google Spreadsheet upload",
                IsChecked = config.UploadEnabled
            };
            enableUpload.Click += (sender, args) =>
            {
                config.UploadEnabled = !config.UploadEnabled;
                enableUpload.IsChecked = config.UploadEnabled;
                
                if (config.UploadEnabled) BgMatchSpreadsheetConnector.ConnectToGoogle(config);

                config.save();
            };

            m.Items.Add(enableUpload);

            return m;
        }

        private void ShowForm()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(config.CsvLocation),
                FileName = Path.GetFileName(config.CsvLocation)
            };
            dialog.ShowDialog();
            config.CsvLocation = dialog.FileName;
            config.save();
        }

    }

}
