using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Media;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;

namespace BattlegroundsMatchData
{
    public class Config
    {
        public static readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\Plugins\BattlegroundsMatchData\BattlegroundsMatchData.config";
        public string CsvLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\BattlegroundsMatchData.csv";
        public bool UploadEnabled = true;
        public bool TestUpload = false;
        public string SheetForMyEndingBoard = "Sheet1";
        public string SheetForAllBoards = "Boards";
        public int TurnToStartTrackingAllBoards = 7;
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
        private Flyout _settingsFlyout;
        private BgMatchDataSettingsControl _settingsControl;

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            GameEvents.OnGameStart.Add(BgMatchData.GameStart);
            GameEvents.OnTurnStart.Add(BgMatchData.TurnStart);
            GameEvents.OnGameEnd.Add(BgMatchData.GameEnd);
            GameEvents.OnInMenu.Add(BgMatchData.InMenu);
            GameEvents.OnPlayerPlay.Add(BgMatchData.PlayerPlay);

            try
            {
                // load config from file, if available
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Config._configLocation));
            }
            catch
            {
                // create config file
                config = new Config();
                config.save();
            }

            BgMatchData.OnLoad(config);

            // connect to Google            
            if (config.UploadEnabled) BgMatchSpreadsheetConnector.ConnectToGoogle(config);

            // create overlay and insert into HDT overlay
            _overlay = new BgMatchOverlay();
            StackPanel BgsTopBar = (StackPanel)Core.OverlayWindow.FindName("BgsTopBar");
            BgsTopBar.Children.Insert(1, _overlay);
            BgMatchData.Overlay = _overlay;

            // create settings flyout
            _settingsFlyout = new Flyout();
            _settingsFlyout.Name = "BgSettingsFlyout";
            _settingsFlyout.Position = Position.Left;
            Panel.SetZIndex(_settingsFlyout, 100);
            _settingsFlyout.Header = "Battlegrounds Match Data Settings";
            _settingsControl = new BgMatchDataSettingsControl(config);
            _settingsFlyout.Content = _settingsControl;
            _settingsFlyout.ClosingFinished += (sender, args) =>
            {
                config.UploadEnabled = (bool)_settingsControl.UploadToggle.IsChecked;
                config.CsvLocation = _settingsControl.CsvLocation.Text;
                config.CredentialLocation = _settingsControl.CredentialLocation.Text;
                config.SpreadsheetId = _settingsControl.SpreadsheetID.Text;
                config.TurnToStartTrackingAllBoards = Int32.Parse(_settingsControl.TurnToTrack.Text);
                config.save();
            };
            Core.MainWindow.Flyouts.Items.Add(_settingsFlyout);

            //_settingsFlyout.IsOpen = true;
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
            _settingsFlyout.IsOpen = true;
        }

        public void OnUpdate()
        {
            BgMatchData.Update();
        }

        public string Name => "Battlegrounds Match Data";

        public string Description => "Save your match statistics in a local CSV file or in a Google Sheet online. Tracks the hero, ending position, minions, and the turns to reach tavern tiers for each match.";

        public string ButtonText => "Settings";

        public string Author => "JawsLouis";

        public Version Version => new Version(0, 3, 2);

        public MenuItem MenuItem => CreateMenu();


        private MenuItem CreateMenu()
        {
            MenuItem m = new MenuItem { Header = "Battlegrounds Match Data Settings" };

            m.Click += (sender, args) =>
            {
                _settingsFlyout.IsOpen = true;
            };

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
