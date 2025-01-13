using CommunityToolkit.Mvvm.Input;
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

public partial class TetrisGame : INotifyPropertyChanged
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    Spielfeld spielfeld;
    SpawnManager spawnManager;
    BlockController blockController;
    public DispatcherTimer dp = new DispatcherTimer();
    public GridBackground GridBackground { get; set; }
    Block aktBlock;
    bool canHold = true;
    bool pausiert = false;
    bool gameActive = false;
    double falltime;
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

    public TetrisGame()
    {
        GridBackground = new GridBackground();
        mainWindow.Loaded += (s, e) => GridBackground.DrawGrid();
        //dp.Interval = new TimeSpan(0, 0, 0, 0, 500);
        dp.Interval = TimeSpan.FromMilliseconds(500);
        dp.Tick += Dp_Tick;
    }

    [RelayCommand]
    public void Start()
    {
        mainWindow.start.Visibility = Visibility.Hidden;
        mainWindow.gameOverText.Visibility = Visibility.Hidden;
        Score = 0;
        spielfeld = new Spielfeld();
        spawnManager = new SpawnManager();
        blockController = new BlockController();
        gameActive = true;
        falltime = 500;
        mainWindow.blocks.Children.Clear();
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

    [RelayCommand]
    public void Pause()
    {
        if (gameActive)
        {
            pausiert = !pausiert;
            if (pausiert)
            {
                dp.Stop();
                mainWindow.pausiertText.Visibility = Visibility.Visible;
            }
            else
            {
                dp.Start();
                mainWindow.pausiertText.Visibility = Visibility.Hidden;
            }
        }
    }

    [RelayCommand]
    public void MoveLeft()
    {
        if(gameActive && !pausiert && spielfeld.IsFree(aktBlock, "left"))
        {
            aktBlock.MoveHorizontal(-1);
        }
    }

    [RelayCommand]
    public void MoveRight()
    {
        if (gameActive && !pausiert && spielfeld.IsFree(aktBlock, "right"))
        {
            aktBlock.MoveHorizontal(1);
        }
    }

    [RelayCommand]
    public void Rotate()
    {
        if (gameActive && !pausiert && aktBlock.Type != "2x2")
        {
            aktBlock = blockController.ValidRotation(aktBlock, spielfeld);
        }
    }

    [RelayCommand]
    public void HardDrop()
    {
        if (gameActive && !pausiert)
        {
            while (spielfeld.IsFree(aktBlock, "down"))
            {
                aktBlock.MoveVertical(-1);
            }
            BlockPlatzieren();
        }
    }

    [RelayCommand]
    public void FastFall()
    {
        if (gameActive && !pausiert && spielfeld.IsFree(aktBlock, "down"))
        {
            aktBlock.MoveVertical(-1);
        }
    }
    
    [RelayCommand]
    public void HoldBlock()
    {
        if (gameActive && !pausiert && canHold)
        {
            aktBlock = blockController.Hold(aktBlock);
            if (aktBlock == null)
            {
                aktBlock = spawnManager.SpawnNewBlock();
            }
            canHold = false;
        }
    }

    void BlockPlatzieren()
    {
        spielfeld.PlaceBlock(aktBlock);
        Score += spielfeld.RowsComplete * 10;
        canHold = true;

        if(falltime > 100)
        {
            falltime--;
            dp.Interval = TimeSpan.FromMilliseconds(falltime);
        }

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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [RelayCommand]
    public void ExitGame()
    {
        App.Current.Shutdown();
    }
}
