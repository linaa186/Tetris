using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris;

public class TetrisGame
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    Spielfeld spielfeld = new Spielfeld();
    SpawnManager spawnManager = new SpawnManager();
    DispatcherTimer dp = new DispatcherTimer();
    Block aktBlock;
    bool gameActive = false;
    public Sound Sound { get; private set; } = new Sound();

    public void Start()
    {
        mainWindow.start.Visibility = Visibility.Hidden;
        gameActive = true;
        dp.Interval = new TimeSpan(0, 0, 0, 0, 500);
        dp.Tick += Dp_Tick;
        dp.Start();
        aktBlock = spawnManager.SpawnNewBlock();
    }

    private void Dp_Tick(object? sender, EventArgs e)
    {
        if (spielfeld.IsFree(aktBlock, "down"))
        {
            Canvas.SetTop(mainWindow.falling, Canvas.GetTop(mainWindow.falling) + 20);
            aktBlock.MoveVertical(-1);
        }
        else
        {
            spielfeld.PlaceBlock(aktBlock);
            mainWindow.falling.Children.RemoveAt(0);
            if (!spielfeld.IsGameOver)
            {
                aktBlock = spawnManager.SpawnNewBlock();
            } else
            {
                gameActive = false;
                dp.Stop();
                var text = new TextBlock
                {
                    Foreground = Brushes.Red,
                    Background = Brushes.Black,
                    FontWeight = FontWeights.Black,
                    FontSize = 35,
                    Width = 200,
                    TextAlignment = TextAlignment.Center,
                    Text = "Game Over"
                };
                Canvas.SetTop(text, 160);
                mainWindow.feld.Children.Add(text);
                Canvas.SetTop(mainWindow.start, 220);
                mainWindow.start.Visibility = Visibility.Visible;
            }
        }
    }

    public void MoveBlock(KeyEventArgs e)
    {
        if (gameActive)
        {
            var xPos = Canvas.GetLeft(mainWindow.falling);
            if (e.Key == Key.Left && spielfeld.IsFree(aktBlock, "left"))
            {
                Canvas.SetLeft(mainWindow.falling, xPos - 20);
                aktBlock.MoveHorizontal(-1);
            }
            else if (e.Key == Key.Right && spielfeld.IsFree(aktBlock, "right"))
            {
                Canvas.SetLeft(mainWindow.falling, xPos + 20);
                aktBlock.MoveHorizontal(1);
            }
            else if (e.Key == Key.Up && aktBlock.Type != "2x2")
            {
                RotaionHilfsmethode();
            }
            else if (e.Key == Key.Space)
            {
                while (spielfeld.IsFree(aktBlock, "down"))
                {
                    aktBlock.MoveVertical(-1);
                }
            }
            else if (e.Key == Key.Down && spielfeld.IsFree(aktBlock, "down"))
            {
                Canvas.SetTop(mainWindow.falling, Canvas.GetTop(mainWindow.falling) + 20);
                aktBlock.MoveVertical(-1);
            }
        }
    }

    void RotaionHilfsmethode()
    {
        var xPos = Canvas.GetLeft(mainWindow.falling);
        var yPos = Canvas.GetTop(mainWindow.falling);
        Rotate(1);
        while (aktBlock.cubes[1].CubePosX < 0 || aktBlock.cubes[2].CubePosX < 0 || aktBlock.cubes[3].CubePosX < 0)
        {
            xPos += 20;
            Canvas.SetLeft(mainWindow.falling, xPos);
            aktBlock.MoveHorizontal(1);
        }
        while (aktBlock.cubes[1].CubePosX > 9 || aktBlock.cubes[2].CubePosX > 9 || aktBlock.cubes[3].CubePosX > 9)
        {
            xPos -= 20;
            Canvas.SetLeft(mainWindow.falling, xPos);
            aktBlock.MoveHorizontal(-1);
        }
        while (aktBlock.cubes[1].CubePosY < 0 || aktBlock.cubes[2].CubePosY < 0 || aktBlock.cubes[3].CubePosY < 0)
        {
            yPos -= 20;
            Canvas.SetTop(mainWindow.falling, yPos);
            aktBlock.MoveVertical(1);
        }
        for (int i = 0; i < 2; i++)
        {
            if (!spielfeld.IsFree(aktBlock, "current"))
            {
                if (spielfeld.IsFree(aktBlock, "left"))
                {
                    xPos -= 20;
                    Canvas.SetLeft(mainWindow.falling, xPos);
                    aktBlock.MoveHorizontal(-1);
                }
                else if (spielfeld.IsFree(aktBlock, "right"))
                {
                    xPos += 20;
                    Canvas.SetLeft(mainWindow.falling, xPos);
                    aktBlock.MoveHorizontal(+1);
                }
            }
        }
        if (!spielfeld.IsFree(aktBlock, "current"))
        {
            Rotate(-1);
        }
    }

    void Rotate(int direction)
    {
        mainWindow.rotation.Angle += 90 * direction;
        for (int i = 1; i < 4; i++)
        {
            int xi = aktBlock.cubes[i].CubePosX;
            int yi = aktBlock.cubes[i].CubePosY;
            int x0 = aktBlock.cubes[0].CubePosX;
            int y0 = aktBlock.cubes[0].CubePosY;

            if (xi == x0)
            {
                xi += (yi - y0) * direction;
                yi = y0;
            } else if (yi == y0)
            {
                yi -= (xi - x0) * direction;
                xi = x0;
            } else if(xi < x0 && yi > y0)
            {
                xi += 2 * direction;
            } else if(xi > x0 && yi > y0)
            {
                yi -= 2 * direction;
            } else if(xi > x0 && yi < y0)
            {
                xi -= 2 * direction;
            } else if(xi < x0 && yi < y0)
            {
                yi += 2 * direction;
            }

            aktBlock.cubes[i].CubePosX = xi;
            aktBlock.cubes[i].CubePosY = yi;
        }
    }
}
