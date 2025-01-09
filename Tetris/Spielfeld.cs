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
    public int RowsComplete { get; set; }

    public Spielfeld()
    {
        for (int i = 0; i < 22; i++)
        {
            reihen.Add(new Cube[10]);
        }
    }

    public void PlaceBlock(Block block)
    {
        RowsComplete = 0;
        foreach (Cube c in block.cubes)
        {
            Cube temp = c;
            reihen[c.CubePosY][c.CubePosX] = temp;
        }
        CompleteRow();
        foreach (Cube c in block.cubes)
        {
            if (c.CubePosY > 16)
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
                    if (IstImFeld(cube) && cube.CubePosY > 0 && reihen[cube.CubePosY - 1][cube.CubePosX] == null)
                    {
                        free = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case "left":
                    if (IstImFeld(cube) && cube.CubePosX > 0 && reihen[cube.CubePosY][cube.CubePosX - 1] == null)
                    {
                        free = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case "right":
                    if (IstImFeld(cube) && cube.CubePosX < 9 && reihen[cube.CubePosY][cube.CubePosX + 1] == null)
                    {
                        free = true;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case "current":
                    if (IstImFeld(cube) && reihen[cube.CubePosY][cube.CubePosX] == null)
                    {
                        free = true;
                    }
                    else
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
        for (int i = 0; i < reihen.Count; i++)
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
                RowsComplete++;
                DeleteRow(i);
                i--;
            }
        }
    }

    private void DeleteRow(int index)
    {
        for (int i = 0; i < mainWindow.blocks.Children.Count; i++)
        {
            if (Canvas.GetBottom(mainWindow.blocks.Children[i]) / 20 == index && Canvas.GetLeft(mainWindow.blocks.Children[i]) >= 0)
            {
                mainWindow.blocks.Children.RemoveAt(i);
                i--;
            }
            else if (Canvas.GetBottom(mainWindow.blocks.Children[i]) / 20 > index && Canvas.GetLeft(mainWindow.blocks.Children[i]) >= 0)
            {
                var yPos = Canvas.GetBottom(mainWindow.blocks.Children[i]);
                Canvas.SetBottom(mainWindow.blocks.Children[i], yPos - 20);
            }
        }
        reihen.RemoveAt(index);
        reihen.Add(new Cube[10]);
    }

    private bool IstImFeld(Cube cube)
    {
        if(cube.CubePosX >= 0 && cube.CubePosX <= 9 && cube.CubePosY >= 0 && cube.CubePosY <= 19)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
