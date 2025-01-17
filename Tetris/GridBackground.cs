using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris;

public class GridBackground
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

    public void DrawGrid()
    {
        for (int x = 0; x < mainWindow.feld.ActualWidth; x += 20)
        {
            var line = new Line
            {
                X1 = x,
                Y1 = 0,
                X2 = x,
                Y2 = mainWindow.feld.ActualHeight,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5,
            };
            mainWindow.feld.Children.Add(line);
        }

        for (int y = 0; y < mainWindow.feld.ActualHeight; y += 20)
        {
            var line = new Line
            {
                X1 = 0,
                Y1 = y,
                X2 = mainWindow.feld.ActualWidth,
                Y2 = y,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5
            };
            mainWindow.feld.Children.Add(line);
        }
    }
}
