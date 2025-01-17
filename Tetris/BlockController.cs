namespace Tetris;

public class BlockController
{
    Block holdBlock;

    public Block Hold(Block aktBlock)
    {
        if (holdBlock == null)
        {
            holdBlock = aktBlock;
            aktBlock = null;
        }
        else
        {
            var block = holdBlock;
            holdBlock = aktBlock;
            aktBlock = block;
            aktBlock.SetStartPosition();
        }
        holdBlock.SetStartPosition();
        foreach (var c in holdBlock.cubes)
        {
            c.CubePosX -= 14;
            c.CubePosY -= 3;
        }
        return aktBlock;
    }

    public Block ValidRotation(Block aktBlock, Spielfeld spielfeld)
    {
        Rotate(1, aktBlock);
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
            aktBlock = Rotate(-1, aktBlock);
        }
        return aktBlock;
    }

    Block Rotate(int direction, Block aktBlock)
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
        return aktBlock;
    }
}
