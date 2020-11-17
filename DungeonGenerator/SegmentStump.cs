using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentStump   // placeholder for a segment in the making
{
    public PlannedSegment Parent;  
    public SegmentCellOriented ZeroPosRot; // position and facing in global planning grid
    public int StumpRole;    //what role does this stump and its future segment play in dungeon plan (main path, sideway, etc.)
    public int PathDepth; //distance from main path if branch      OR     length of main path if part of it
    public int PathWeight;  //^ but in cells generated instead of number of segments


    public SegmentStump(PlannedSegment parent, SegmentCellOriented zeroposrot, int role, int pathdepth, int pathweight)
    {
        Parent = parent;
        ZeroPosRot = zeroposrot;
        StumpRole = role;
        PathDepth = pathdepth;
        PathWeight = pathweight;
    }

    public static List<SegmentStump> MakeStumpsForOpenings(PlannedSegment segment)
    {
        List<SegmentStump> result = new List<SegmentStump>();
        SegmentStump stump;
        foreach (SegmentCellOriented opening in segment.Scheme.Openings[segment.ZeroPosRot.Facing])
        {
            stump = new SegmentStump(   segment,
                                        new SegmentCellOriented(segment.ZeroPosRot.U + opening.U,
                                                                segment.ZeroPosRot.R + opening.R,
                                                                opening.Facing,
                                                                opening.Type,
                                                                segment.ZeroPosRot.Altitude + opening.Altitude),
                                        segment.SegmentRole,
                                        segment.PathDepth,
                                        segment.PathWeight);
            result.Add(stump);
        }
        return result;
    }
}
