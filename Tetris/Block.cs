using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris;

public class Block
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public string Type { get; set; }
    public Cube[] cubes = new Cube[4];

    public Block(string type)
    {
        Type = type;
        BuildBlock();
    }

    public void MoveHorizontal(int direction)
    {
        foreach (Cube c in cubes)
        {
            c.CubePosX += direction;
        }
    }

    public void MoveVertical(int direction)
    {
        foreach (Cube c in cubes)
        {
            c.CubePosY += direction;
        }
    }

    public void BuildBlock()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            cubes[i] = new Cube(Type);
        }
        switch (Type)
        {
            case "rightL":
                cubes[0].CubePosX = 5; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 5; cubes[1].CubePosY = 19;
                cubes[2].CubePosX = 4; cubes[2].CubePosY = 19;
                cubes[3].CubePosX = 5; cubes[3].CubePosY = 17;
                break;
            case "leftL":
                cubes[0].CubePosX = 5; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 5; cubes[1].CubePosY = 19;
                cubes[2].CubePosX = 6; cubes[2].CubePosY = 19;
                cubes[3].CubePosX = 5; cubes[3].CubePosY = 17;
                break;
            case "rightZ":
                cubes[0].CubePosX = 5; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 5; cubes[1].CubePosY = 19;
                cubes[2].CubePosX = 4; cubes[2].CubePosY = 19;
                cubes[3].CubePosX = 6; cubes[3].CubePosY = 18;
                break;
            case "leftZ":
                cubes[0].CubePosX = 5; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 5; cubes[1].CubePosY = 19;
                cubes[2].CubePosX = 6; cubes[2].CubePosY = 19;
                cubes[3].CubePosX = 4; cubes[3].CubePosY = 18;
                break;
            case "1x4":
                cubes[0].CubePosX = 4; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 3; cubes[1].CubePosY = 18;
                cubes[2].CubePosX = 5; cubes[2].CubePosY = 18;
                cubes[3].CubePosX = 6; cubes[3].CubePosY = 18;
                break;
            case "2x2":
                cubes[0].CubePosX = 4; cubes[0].CubePosY = 19;
                cubes[1].CubePosX = 4; cubes[1].CubePosY = 18;
                cubes[2].CubePosX = 5; cubes[2].CubePosY = 18;
                cubes[3].CubePosX = 5; cubes[3].CubePosY = 19;
                break;
            case "T":
                cubes[0].CubePosX = 5; cubes[0].CubePosY = 18;
                cubes[1].CubePosX = 5; cubes[1].CubePosY = 19;
                cubes[2].CubePosX = 5; cubes[2].CubePosY = 17;
                cubes[3].CubePosX = 6; cubes[3].CubePosY = 18;
                break;
        }
    }
}
