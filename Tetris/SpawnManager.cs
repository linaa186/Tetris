using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Tetris;

public class SpawnManager
{
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public Block[] blocks = new Block[7];
    private Block nextBlock;

    public SpawnManager()
    {
        BuildBlocks();
    }

    private void BuildBlocks()
    {
        blocks[0] = new Block("rightL");
        blocks[1] = new Block("leftL");
        blocks[2] = new Block("rightZ");
        blocks[3] = new Block("leftZ");
        blocks[4] = new Block("1x4");
        blocks[5] = new Block("2x2");
        blocks[6] = new Block("T");
    }

    public Block SpawnNewBlock()
    {
        //mainWindow.rotation.Angle = 0;
        Block aktBlock = nextBlock;
        foreach (var c in nextBlock.cubes)
        {
            c.CubePosX -= 9;
            c.CubePosY += 2;
        }
        NextBlock();
        return aktBlock;
    }

    public void NextBlock()
    {
        //mainWindow.nextBlock.Children.Clear();
        Random rnd = new Random();
        nextBlock = new Block(blocks[rnd.Next(0, 7)].Type);
        foreach(var c in nextBlock.cubes)
        {
            c.CreateCube();
            c.CubePosX += 9;
            c.CubePosY -= 2;
        }
    }
}
