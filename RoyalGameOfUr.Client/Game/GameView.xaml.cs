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
using System.Windows.Shapes;

namespace RoyalGameOfUr.Client.Game
{
    /// <summary>
    /// Interaktionslogik für GameView.xaml
    /// </summary>
    public partial class GameView : Window
    {
        private readonly GameViewModel vm;

        public GameView(StrictGame game)
        {
            InitializeComponent();

            vm = new GameViewModel(game);
            DataContext = vm;

            game.Moved += (s, a) => RepaintAll();
        }

        public void RepaintAll()
        {
            BoardGrid.InvalidateVisual();
        }
    }
}
