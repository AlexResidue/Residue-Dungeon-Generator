using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SegmentCell    // describes a dungeon/segment cell in dungeon planning grid 
{
    public int U = 0;   //+ up   - down
    public int R = 0;   //+ right   - left

    public SegmentCell(int u, int r)
    {
        U=u;
        R=r;
    }
}


public class SegmentCellOriented    // SegmentCell that can hold info describing door orientation, type and altitude
{
    public int U = 0;   //+ up   - down
    public int R = 0;   //+ right   - left
    public int Facing = CellFacing.Up;    
    public int Type = 0;        // 0 - no door
    public float Altitude = 0f;  // height of door relative to cell entrance (or the entrance door to dungeon if global)

    public SegmentCellOriented(int u, int r, int facing, int type, float altitude)
    {
        U = u;
        R = r;
        Facing = facing;
        Type = type;
        Altitude = altitude;
    }

}
