using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlannedSegment
{
    public ArchivedSegment Scheme;  // reference to an arhived segment in codex
    public SegmentCellOriented ZeroPosRot; // position and facing of (0.0) cell in global planning grid
    //public SegmentCell PivotInGlobal; //pivot position in global planning grid
    public PlannedSegment Parent;
    public List<PlannedSegment> Children;
    public int SegmentRole;
    public int PathDepth;   //distance from main path if dungeon branch or distance as main path if part of it
    public int PathWeight;  //cells generated so far as branch from main path or cells generated so far as main path if part of it

    public void PlanFromScheme(ArchivedSegment scheme, SegmentStump gridStump)
    {
        Scheme = scheme;
        ZeroPosRot = new SegmentCellOriented(gridStump.ZeroPosRot.U,
                                             gridStump.ZeroPosRot.R,
                                             gridStump.ZeroPosRot.Facing,
                                             gridStump.ZeroPosRot.Type,
                                             gridStump.ZeroPosRot.Altitude);
        SegmentRole = gridStump.StumpRole;
        PathDepth = gridStump.PathDepth + 1;
        PathWeight = gridStump.PathWeight + Scheme.Weight;
        
        this.HasParent(gridStump.Parent);
        if(gridStump.Parent != null)
            gridStump.Parent.IsParentOf(this);
    }
    public void IsParentOf(PlannedSegment child)
    {
        if(Children == null)
            Children = new List<PlannedSegment>();
        Children.Add(child);
    }
    public void HasParent(PlannedSegment parent)
    {
        Parent = parent;
    }
}
