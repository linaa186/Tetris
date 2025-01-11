using System;
using System.Collections.Generic;
using System.ComponentModel;
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

public class TetrisGame : INotifyPropertyChanged
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    Spielfeld spielfeld;
    SpawnManager spawnManager;
    public DispatcherTimer dp = new DispatcherTimer();
    Block aktBlock;
    bool gameActive = false;
    Block holdBlock;
    bool canHold = true;
    bool pausiert = false;
    private int score;

    public int Score
    {
        get { return score; }
        set
        {
            if (score != value)
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
    }
    //public Sound Sound { get; private set; } = new Sound();
    public GridBackground GridBackground { get; set; }

    public TetrisGame()
    {
        GridBackground = new GridBackground();
        mainWindow.Loaded += (s, e) => GridBackground.DrawGrid();
        dp.Interval = new TimeSpan(0, 0, 0, 0, 500);
        dp.Tick += Dp_Tick;
    }

    public void Start()
    {
        mainWindow.start.Visibility = Visibility.Hidden;
        mainWindow.gameOverText.Visibility = Visibility.Hidden;
        Score = 0;
        spielfeld = new Spielfeld();
        spawnManager = new SpawnManager();
        mainWindow.blocks.Children.Clear();
        gameActive = true;
        spawnManager.NextBlock();
        aktBlock = spawnManager.SpawnNewBlock();
        dp.Start();
    }

    private void Dp_Tick(object? sender, EventArgs e)
    {
        if (spielfeld.IsFree(aktBlock, "down"))
        {
            aktBlock.MoveVertical(-1);
        }
        else
        {
            BlockPlatzieren();
        }
    }

    public void MoveBlock(KeyEventArgs e)
    {
        if (gameActive && e.Key == Key.Escape)
        {
            Pausieren();
        }
        if (gameActive && !pausiert)
        {
            if (e.Key == Key.Left && spielfeld.IsFree(aktBlock, "left"))
            {
                aktBlock.MoveHorizontal(-1);
            }
            else if (e.Key == Key.Right && spielfeld.IsFree(aktBlock, "right"))
            {
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
                BlockPlatzieren();
            }
            else if (e.Key == Key.Down && spielfeld.IsFree(aktBlock, "down"))
            {
                aktBlock.MoveVertical(-1);
            } else if (e.Key == Key.RightCtrl && canHold)
            {
                HoldBlock();
            }
        }
    }

    void BlockPlatzieren()
    {
        spielfeld.PlaceBlock(aktBlock);
        Score += spielfeld.RowsComplete * 10;
        canHold = true;
        if (!spielfeld.IsGameOver)
        {
            aktBlock = spawnManager.SpawnNewBlock();
        }
        else
        {
            gameActive = false;
            mainWindow.gameOverText.Visibility = Visibility.Visible;
            Canvas.SetTop(mainWindow.start, 220);
            mainWindow.start.Visibility = Visibility.Visible;
            dp.Stop();
        }
    }

    void Pausieren()
    {
        pausiert = !pausiert;
        if (pausiert)
        {
            dp.Stop();
            mainWindow.pausiertText.Visibility = Visibility.Visible;
        } else
        {
            dp.Start();
            mainWindow.pausiertText.Visibility = Visibility.Hidden;
        }
    }

    void RotaionHilfsmethode()
    {
        Rotate(1);
        while (aktBlock.cubes[1].CubePosX < 0 || aktBlock.cubes[2].CubePosX < 0 || aktBlock.cubes[3].CubePosX < 0)
        {
            aktBlock.MoveHorizontal(1);
        }
        while (aktBlock.cubes[1].CubePosX > 9 || aktBlock.cubes[2].CubePosX > 9 || aktBlock.cubes[3].CubePosX > 9)
        {
            aktBlock.MoveHorizontal(-1);
        }
        while (aktBlock.cubes[1].CubePosY < 0 || aktBlock.cubes[2].CubePosY < 0 || aktBlock.cubes[3].CubePosY < 0)
        {
            aktBlock.MoveVertical(1);
        }
        for (int i = 0; i < 2; i++)
        {
            if (!spielfeld.IsFree(aktBlock, "current"))
            {
                if (spielfeld.IsFree(aktBlock, "left"))
                {
                    aktBlock.MoveHorizontal(-1);
                }
                else if (spielfeld.IsFree(aktBlock, "right"))
                {
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
            }
            else if (yi == y0)
            {
                yi -= (xi - x0) * direction;
                xi = x0;
            }
            else if (xi < x0 && yi > y0)
            {
                xi += 2 * direction;
            }
            else if (xi > x0 && yi > y0)
            {
                yi -= 2 * direction;
            }
            else if (xi > x0 && yi < y0)
            {
                xi -= 2 * direction;
            }
            else if (xi < x0 && yi < y0)
            {
                yi += 2 * direction;
            }

            aktBlock.cubes[i].CubePosX = xi;
            aktBlock.cubes[i].CubePosY = yi;
        }
    }

    void HoldBlock()
    {
        if (holdBlock == null)
        {
            holdBlock = aktBlock;
            aktBlock = spawnManager.SpawnNewBlock();
        } else
        {
            var block = holdBlock;
            holdBlock = aktBlock;
            aktBlock = block;
            aktBlock.SetPosition();
        }
        holdBlock.SetPosition();
        foreach (var c in holdBlock.cubes)
        {
            c.CubePosX -= 9;
            c.CubePosY -= 2;
        }
        canHold = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
