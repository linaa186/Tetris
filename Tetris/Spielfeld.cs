using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Tetris;

public class Spielfeld
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public List<Cube[]> reihen = new List<Cube[]>();
    public bool IsGameOver { get; set; }

    public Spielfeld()
    {
        for (int i = 0; i < 22; i++)
        {
            reihen.Add(new Cube[10]);
        }
    }

    public void PlaceBlock(Block block)
    {
        foreach (Cube c in block.cubes)
        {
            var temp = new Cube(block.Type);
            reihen[c.CubePosY][c.CubePosX] = temp;
            temp.CreateCube(c.CubePosX, c.CubePosY);
        }
        CompleteRow();
        foreach(Cube c in block.cubes)
        {
            if(c.CubePosY > 17)
            {
                IsGameOver = true;
            }
        }
    }

    public bool IsFree(Block block, string direction)
    {
        bool free = true;
        foreach (Cube cube in block.cubes)
        {
            switch (direction)
            {
                case "down":
                    if (cube.CubePosY > 0 && reihen[cube.CubePosY-1][cube.CubePosX] == null)
                    {
                        free = true;
                    } else
                    {
                        return false;
                    }
                    break;
                case "left":
                    if(cube.CubePosX > 0 && reihen[cube.CubePosY][cube.CubePosX-1] == null)
                    {
                        free = true;
                    } else
                    {
                        return false;
                    }
                    break;
                case "right":
                    if(cube.CubePosX < 9 && reihen[cube.CubePosY][cube.CubePosX + 1] == null)
                    {
                        free = true;
                    } else
                    {
                        return false;
                    }
                    break;
                case "current":
                    if (reihen[cube.CubePosY][cube.CubePosX] == null)
                    {
                        free = true;
                    } else
                    {
                        return false;
                    }
                    break;
            }
        }
        return free;
    }

    private void CompleteRow()
    {
        for(int i = 0; i < reihen.Count; i++)
        {
            bool complete = true;
            for (int j = 0; j < reihen[i].Length; j++)
            {
                if (reihen[i][j] == null)
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                DeleteRow(i);
                i--;
            }
        }
    }

    private void DeleteRow(int index)
    {
        for(int i = 0; i < mainWindow.feld.Children.Count; i++)
        {
            if (Canvas.GetBottom(mainWindow.feld.Children[i]) < index * 20 + 1 && Canvas.GetBottom(mainWindow.feld.Children[i]) > index * 20 - 1)
            {
                mainWindow.feld.Children.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < mainWindow.feld.Children.Count; i++)
        {
            if (Canvas.GetBottom(mainWindow.feld.Children[i]) > index * 20)
            {
                var yPos = Canvas.GetBottom(mainWindow.feld.Children[i]);
                Canvas.SetBottom(mainWindow.feld.Children[i], yPos - 20);
            }
        }
        reihen.RemoveAt(index);
        reihen.Add(new Cube[10]);
    }
}
