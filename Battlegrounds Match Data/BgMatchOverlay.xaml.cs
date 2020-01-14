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
            Hide();
        }
        public void UpdatePosition()
        {
            Canvas.SetBottom(this, Core.OverlayWindow.Height * 10 / 100);
            Canvas.SetRight(this, Core.OverlayWindow.Width * 10 / 100);
        }

        public void UpdateTurn(int turn)
        {
            TurnLabel.Content = "Turn " + turn;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
