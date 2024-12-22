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
        nextBlock = NextBlock();
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
        mainWindow.rotation.Angle = 0;
        Block aktBlock = nextBlock;
        aktBlock.BuildBlock();
        Canvas.SetLeft(mainWindow.falling, 80);
        if(aktBlock.Type == "1x4")
        {
            Canvas.SetLeft(mainWindow.falling, 60);
        }
        Canvas.SetTop(mainWindow.falling, 0);
        var child = mainWindow.nextBlock.Children[0];
        mainWindow.nextBlock.Children.Clear();
        mainWindow.falling.Children.Add(child);
        nextBlock = NextBlock();
        return aktBlock;
    }

    private Block NextBlock()
    {
        Random rnd = new Random();
        nextBlock = blocks[rnd.Next(0, 7)];
        //nextBlock = blocks[4];
        switch (nextBlock.Type)
        {
            case "rightL":
                mainWindow.nextBlock.Children.Add(new RightL());
                break;
            case "leftL":
                mainWindow.nextBlock.Children.Add(new LeftL());
                break;
            case "rightZ":
                mainWindow.nextBlock.Children.Add(new RightZ());
                break;
            case "leftZ":
                mainWindow.nextBlock.Children.Add(new LeftZ());
                break;
            case "1x4":
                mainWindow.nextBlock.Children.Add(new Block1x4());
                break;
            case "2x2":
                mainWindow.nextBlock.Children.Add(new Block2x2());
                break;
            case "T":
                mainWindow.nextBlock.Children.Add(new BlockT());
                break;
        }
        return nextBlock;
    }
}
