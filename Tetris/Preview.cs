using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris;

public class Preview
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public void UpdatePreview(Block b, Spielfeld spielfeld)
    {
        ClearPreview();
        var block = new Block(b.Type);
        for(int i = 0; i < block.cubes.Length; i++)
        {
            block.cubes[i].CubePosX = b.cubes[i].CubePosX;
            block.cubes[i].CubePosY = b.cubes[i].CubePosY;
        }
        while (spielfeld.IsFree(block, "down"))
        {
            block.MoveVertical(-1);
        }
        foreach (Cube c in block.cubes)
        {
            var cube = new System.Windows.Shapes.Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Gray,
                Opacity = 0.5
            };
            Canvas.SetLeft(cube, c.CubePosX * 20);
            Canvas.SetBottom(cube, c.CubePosY * 20);
            mainWindow.preview.Children.Add(cube);
        }
    }

    public void ClearPreview()
    {
        mainWindow.preview.Children.Clear();
    }
}
