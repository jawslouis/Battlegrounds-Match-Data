using System;
using System.IO;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Microsoft.Win32;

namespace BattlegroundsMatchData
{
    public class Config
    {
        public string SaveLocation;
    }

    public class BgMatchDataPlugin : IPlugin
    {

        private string _saveLocation;
        private readonly string _configLocation = Hearthstone_Deck_Tracker.Config.AppDataPath + @"\BattlegroundsMatchData.config";

        public void OnLoad()
        {
            // Triggered upon startup and when the user ticks the plugin on
            GameEvents.OnGameStart.Add(BgMatchData.GameStart);
            GameEvents.OnTurnStart.Add(BgMatchData.TurnStart);
            GameEvents.OnGameEnd.Add(BgMatchData.GameEnd);
            GameEvents.OnInMenu.Add(BgMatchData.InMenu);

            if (File.Exists(_configLocation))
            {
                _saveLocation = File.ReadAllText(_configLocation);
                BgMatchData.CsvLocation = _saveLocation;
            }
            else
            {
                UpdateSaveLocation(Hearthstone_Deck_Tracker.Config.AppDataPath + @"\BattlegroundsMatchData.csv");
            }

            BgMatchData.OnLoad();
        }

        private void UpdateSaveLocation(string loc)
        {
            _saveLocation = loc;
            BgMatchData.CsvLocation = _saveLocation;
            File.WriteAllText(_configLocation, _saveLocation);
        }

        public void OnUnload()
        {
            // Triggered when the user unticks the plugin, however, HDT does not completely unload the plugin.
            // see https://git.io/vxEcH
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

        public Version Version => new Version(0, 1, 0);

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
            
            return m;
        }

        private void ShowForm()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                InitialDirectory = Path.GetDirectoryName(_saveLocation),
                FileName = Path.GetFileName(_saveLocation)
            };
            dialog.ShowDialog();
            UpdateSaveLocation(dialog.FileName);
        }

    }

}
