using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.FlyoutControls.Options.HSReplay;
using Hearthstone_Deck_Tracker.HsReplay.Data;
using Hearthstone_Deck_Tracker.Utility;
using Hearthstone_Deck_Tracker.Utility.Logging;
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
        public SettingsControl(Config c)
        {
            InitializeComponent();
            UpdateConfig(c);
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
        }

        public ICommand OpenBgStatsCommand => new Command(() =>
        {
            string user = Helper.OptionsMain.OptionsHSReplayAccount.Username;
            Helper.TryOpenUrl("http://bgstats.cintrest.com/" + user.Replace("#", "-"));
        });

    }
}
