using System.Windows;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new TetrisGame();
        }
    }
}
