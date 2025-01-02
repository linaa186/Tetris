using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris;

public class Sound
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    MediaPlayer mediaPlayer = new MediaPlayer();

    public Sound()
    {
        mediaPlayer.Open(new Uri("Assets/TetrisSoundtrack.mp3", UriKind.Relative));
        mediaPlayer.MediaEnded += (s, e) => mediaPlayer.Position = TimeSpan.Zero;
        mediaPlayer.Volume = 0.2;
        mediaPlayer.Play();
    }

    //public void Mute()
    //{
    //    if(mediaPlayer.Volume > 0)
    //    {
    //        mediaPlayer.Volume = 0;
    //        mainWindow.mute.Background = Brushes.LightGray;
    //    } else
    //    {
    //        mediaPlayer.Volume = 0.2;
    //        mainWindow.mute.Background = Brushes.LightBlue;
    //    }
    //}
}
