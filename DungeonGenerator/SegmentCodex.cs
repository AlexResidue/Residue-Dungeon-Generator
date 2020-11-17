using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SegmentCodex
{
    

    public static bool Registered = false;
    public static List<ArchivedSegment> AllSegments = new List<ArchivedSegment>();
    public static List<ArchivedSegment>[,] Categoried = new List<ArchivedSegment>[5, DungeonTheme.DungeonThemesAmount];  //category , theme


    private static List<SegmentCell> Occupation;
    private static List<SegmentCellOriented> Openings;
    private static ArchivedSegment Result;

    static SegmentCodex()
    {

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(1,0),
            new SegmentCell(1,-1)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(1,1, CellFacing.Right, DoorType.Generic, 0),
            new SegmentCellOriented(2,-1, CellFacing.Up, DoorType.Generic, 0)
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Branch,
                                            "ExampleBranch1"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(1,0),
            new SegmentCell(1,-1),
            new SegmentCell(1,-2)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(1,1, CellFacing.Right, DoorType.Generic, 0),
            new SegmentCellOriented(2,-2, CellFacing.Up, DoorType.Generic, 0)
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Branch,
                                            "ExampleBranch2"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(0,1)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(1,-1, CellFacing.Up, DoorType.Generic, 0),
            new SegmentCellOriented(1,0, CellFacing.Up, DoorType.Generic, 0),
            new SegmentCellOriented(1,1, CellFacing.Up, DoorType.Generic, 0)
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Branch,
                                            "ExampleBranch3"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(0,1),
            new SegmentCell(1,0),
            new SegmentCell(1,-1),
            new SegmentCell(1,1)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(0,-2, CellFacing.Left, DoorType.Generic, 0),
            new SegmentCellOriented(1,2, CellFacing.Right, DoorType.Generic, 0),
            new SegmentCellOriented(1,-2, CellFacing.Left, DoorType.Generic, 0),
            new SegmentCellOriented(2,-1, CellFacing.Up, DoorType.Generic, 0),
            new SegmentCellOriented(2,0, CellFacing.Up, DoorType.Generic, 0)
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Branch,
                                            "ExampleBranch4"));
        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(1,0),
            new SegmentCell(1,-1)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(0,-2, CellFacing.Left, DoorType.Generic, 0),            
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Corridor,
                                            "ExampleCorridor1"));


        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(0,1, CellFacing.Right, DoorType.Generic, 0),
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Corridor,
                                            "ExampleCorridor2"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(1,0, CellFacing.Up, DoorType.Generic, 0),
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Corridor,
                                            "ExampleCorridor3"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,1),
            new SegmentCell(1,0),
            new SegmentCell(1,1)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(2,0, CellFacing.Up, DoorType.Generic, 0),
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Corridor,
                                            "ExampleCorridor4"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(1,0),
            new SegmentCell(0,-1),
            new SegmentCell(1,-1),
            new SegmentCell(0,-2),
            new SegmentCell(1,-2),
            new SegmentCell(2,-1),
            new SegmentCell(3,-1),
            new SegmentCell(2,-2),
            new SegmentCell(3,-2),
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(3,0, CellFacing.Right, DoorType.Generic, 0),
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Corridor,
                                            "ExampleCorridor5"));

        
        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0),
            new SegmentCell(0,-1),
            new SegmentCell(1,0),
            new SegmentCell(1,-1)
        };
        Openings = new List<SegmentCellOriented>()
        {
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Deadend,
                                            "ExampleDeadend1"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Deadend,
                                            "ExampleDeadend2"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Deadend,
                                            "ExampleDeadend3"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Deadend,
                                            "ExampleDeadend4"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
            new SegmentCellOriented(1,0, CellFacing.Up, DoorType.Generic, 0),
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Entrance,
                                            "ExampleEntrance1"));

        //-------------------------------------------------------------------------------

        Occupation = new List<SegmentCell>()
        {
            new SegmentCell(0,0)
        };
        Openings = new List<SegmentCellOriented>()
        {
        };
        AllSegments.Add(new ArchivedSegment(Occupation,
                                            Openings,
                                            DoorType.Generic,
                                            DungeonTheme.RockyUnderground,
                                            SegmentCategory.Exit,
                                            "ExampleExit1"));

        //-------------------------------------------------------------------------------

        FormCategories();
    }
    
    public static void FormCategories()
    {
        foreach(ArchivedSegment seg in AllSegments)
        {
            if( Categoried [ seg.Category, seg.Theme] == null)
                Categoried [ seg.Category, seg.Theme] = new List<ArchivedSegment>();
            Categoried [ seg.Category, seg.Theme].Add (seg);
        }
    }

    public static ArchivedSegment GetRandomSegment(int category, int theme, int doorType)
    {
        //Debug.Log("getting segment cat theme: " + category + " " + theme);
        if(category == SegmentCategory.Entrance) 
            return Categoried [category, theme] [ Random.Range(0, Categoried [category, theme].Count) ];
        //if an entrance is needed, then pass random one. Otherwise try matching doors
        do
        { 
            Result = Categoried [category, theme] [ Random.Range(0, Categoried [category, theme].Count) ];
        }while( ( Result.DoorsCombined & DoorType.GetCompatMaskForDoorType(doorType) ) == 0 ); 
        //at least one door in segment must be compatible with door exiting previous segment
        return Result;
    }

}
