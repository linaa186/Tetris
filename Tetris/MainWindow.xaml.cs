using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace Tetris
{
    public partial class MainWindow : Window
    {
        public TetrisGame Game { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Game = new TetrisGame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Game.MoveBlock(e);
        }

        //private void mute_Click(object sender, RoutedEventArgs e)
        //{
        //    Game.Sound.Mute();
        //}

        private void start_Click(object sender, RoutedEventArgs e)
        {
            Game.Start();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
    }
}
