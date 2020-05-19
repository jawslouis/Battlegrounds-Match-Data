using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.FlyoutControls.Options.HSReplay;
using Hearthstone_Deck_Tracker.HsReplay.Data;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattlegroundsMatchData
{
    public partial class SettingsControl : UserControl
    {

        private Action _mount;
        private Action _unmount;
        private Config _config;

        public SettingsControl(Config c, Action mount, Action unmount)
        {
            InitializeComponent();
            _config = c;
            UpdateConfig(c);
            _mount = mount;
            _unmount = unmount;
        }

        public void UpdateConfig(Config c)
        {
            UploadToggle.IsChecked = c.SpreadsheetUploadEnabled;
            CsvLocation.Text = c.CsvGameRecordLocation;
            BoardCsvLocation.Text = c.CsvBoardRecordLocation;
            CredentialLocation.Text = c.CredentialLocation;
            SpreadsheetID.Text = c.SpreadsheetId;
            TurnToTrack.Text = c.TurnToStartTrackingAllBoards.ToString();
            BgStatsToggle.IsChecked = c.GraphqlUploadEnabled;
            BgStatsLink.Command = OpenBgStatsCommand;
            string user = Helper.OptionsMain.OptionsHSReplayAccount.Username;
            BgStatsLinkText.Text = user;
            StatsOverlayToggle.IsChecked = c.showStatsOverlay;
        }

        public ICommand OpenBgStatsCommand => new Command(() =>
        {
            string user = Helper.OptionsMain.OptionsHSReplayAccount.Username;
            Helper.TryOpenUrl("http://bgstats.cintrest.com/" + user.Replace("#", "-"));
        });

        private void Mount(object sender, RoutedEventArgs e)
        {
            _mount();
            _config.showStatsOverlay = true;

        }

        private void Unmount(object sender, RoutedEventArgs e)
        {
            _unmount();
            _config.showStatsOverlay = false;
        }
    }
}
