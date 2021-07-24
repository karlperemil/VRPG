using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBounds
{
    public int minWidth = int.MaxValue;
    public int maxWidth = int.MinValue;
    public int minHeight = int.MaxValue;
    public int maxHeight = int.MinValue;

    public void CalculateTileBounds(List<GridTileConfig> tilesToDivide)
    {
        
        foreach (GridTileConfig tile in tilesToDivide)
        {
            minWidth = tile.xpos < minWidth ? tile.xpos : minWidth;
            maxWidth = tile.xpos > maxWidth ? tile.xpos : maxWidth;
            minHeight = tile.zpos < minHeight ? tile.zpos : minHeight;
            maxHeight = tile.zpos > maxHeight ? tile.zpos : maxHeight;
        }
    }
}
