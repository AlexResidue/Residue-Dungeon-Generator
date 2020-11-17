using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DungeonPlan
{
    public List<PlannedSegment> AllSegments = new List<PlannedSegment>();
    public List<PlannedSegment> MainPath = new List<PlannedSegment>();
    public List<PlannedSegment> SideWays = new List<PlannedSegment>();
    public PlannedSegment StartingSegment;

    public int DungeonWeight = 0;

    public void AddSegment(PlannedSegment seg)
    {
        AllSegments.Add(seg);
        if(seg.SegmentRole == DungeonSegmentRole.MainPath)
        {
            MainPath.Add(seg);
        }else
        {
            SideWays.Add(seg);
        }
        if(StartingSegment == null)
        {
            StartingSegment = seg;
        }
        DungeonWeight += seg.Scheme.Weight;
    }
    
    



    

    


    

}
