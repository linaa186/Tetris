using System;

namespace Tetris;

public class SpawnManager
{
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
        Block aktBlock = nextBlock;
        aktBlock.SetStartPosition();
        NextBlock();
        return aktBlock;
    }

    public void NextBlock()
    {
        Random rnd = new Random();
        nextBlock = new Block(blocks[rnd.Next(0, 7)].Type);
        foreach(var c in nextBlock.cubes)
        {
            c.CreateCube();
            c.CubePosX += 14;
            c.CubePosY -= 3;
        }
    }
}
