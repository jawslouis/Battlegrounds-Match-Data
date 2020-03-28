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

using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace BattlegroundsMatchData
{
    /// <summary>
    /// Interaction logic for BgMatchOverlay.xaml
    /// </summary>
    public partial class BgMatchOverlay : UserControl
    {
        public BgMatchOverlay()
        {
            InitializeComponent();            
        }

        public void UpdateAvgStats(float atk, float health)
        {
            AvgStatsLabel.Content = $"Avg Stats: {atk.ToString("0.0")} / {health.ToString("0.0")}";
        }

        public void UpdateTotalStats(int atk, int health)
        {
            TotalStatsLabel.Content = $"Total Stats: {atk} / {health}";
        }

    }
}
