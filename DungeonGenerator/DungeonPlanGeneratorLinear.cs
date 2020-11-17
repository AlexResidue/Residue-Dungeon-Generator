using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlanGeneratorLinear : DungeonPlanGeneratorBase
{
    //private int SideBranchingEntropy = 0;   //influences chances of sideways branching; rises as branches fail to branch when they should (dungeon topology wont allow it)
                                              //WIP
    public override bool GeneratePlan(out DungeonPlan dungeonPlan)
    {
        dungeonPlan = new DungeonPlan();
        SegmentStump MainPathStump;
        List<SegmentStump> SidewayStumps = new List<SegmentStump>();

        if(PlanningGrid == null)         
            PlanningGrid = new byte[Settings.PlanGridDimensions + 1, Settings.PlanGridDimensions + 1];
        else
            Array.Clear(PlanningGrid, 0, PlanningGrid.Length);


        //ENTRANCE
        PlannedSegment EntrancePlan = new PlannedSegment();
        SegmentStump EntranceStump = new SegmentStump(  null,
                                                        new SegmentCellOriented((int)(Settings.PlanGridDimensions * EntranceMargin.x),
                                                                                (int)(Settings.PlanGridDimensions * EntranceMargin.y),
                                                                                SegmentRotation.Up,
                                                                                DoorType.NonExistant, //no door into first segment
                                                                                0), //starting altitude is considered zero
                                                        DungeonSegmentRole.MainPath,  //Entrance is a start of main path
                                                        0, //dungeon starting segment has 0 depth and weight before it
                                                        0);
        ArchivedSegment schemeChoice = TryFindFittingSegment(SegmentCategory.Entrance, EntranceStump);
        if(schemeChoice == null)
        {
            Debug.Log("Could not plan entrance");
            return false;
        }else
        {
            RegisterInGrid(schemeChoice, EntranceStump);
        }
        EntrancePlan.PlanFromScheme(schemeChoice, EntranceStump);
        dungeonPlan.AddSegment(EntrancePlan);
        List<SegmentStump> OpeningsStumps = SegmentStump.MakeStumpsForOpenings(EntrancePlan);
        ShuffleList<SegmentStump>(OpeningsStumps);
            
        MainPathStump = OpeningsStumps[0];  //first one after shuffle is main, the rest are sideways
        MainPathStump.StumpRole = DungeonSegmentRole.MainPath;
        OpeningsStumps.RemoveAt(0);
        foreach (SegmentStump stump in OpeningsStumps) 
        {
            stump.StumpRole = DungeonSegmentRole.Sideway;
            stump.PathDepth = 0;    //depth and weight are related to sideway now
            stump.PathWeight = 0;
            SidewayStumps.Add(stump);
        }


        //MAIN PATH
        PlannedSegment MainPathPlan;
        int ChosenCategory;
        while( (ChosenCategory = ChooseCategoryForStump(MainPathStump)) != SegmentCategory.Exit )
        {
            MainPathPlan = new PlannedSegment();
            schemeChoice = TryFindFittingSegment(ChosenCategory, MainPathStump);
            if (schemeChoice == null)
            {
                //Debug.Log("Could not plan part of main path at Depth / Weight: " + MainPathStump.PathDepth + " / " + MainPathStump.PathWeight);
                return false;
            }else
            {
                RegisterInGrid(schemeChoice, MainPathStump);
            }
            MainPathPlan.PlanFromScheme(schemeChoice, MainPathStump);
            dungeonPlan.AddSegment(MainPathPlan);
            OpeningsStumps = SegmentStump.MakeStumpsForOpenings(MainPathPlan);
            ShuffleList<SegmentStump>(OpeningsStumps);

            MainPathStump = OpeningsStumps[0];  //first one after shuffle is main, the rest are sideways
            MainPathStump.StumpRole = DungeonSegmentRole.MainPath;
            OpeningsStumps.RemoveAt(0);
            foreach (SegmentStump stump in OpeningsStumps)
            {
                stump.StumpRole = DungeonSegmentRole.Sideway;
                stump.PathDepth = 0;
                stump.PathWeight = 0;
                SidewayStumps.Add(stump);
            }
        }


        //EXIT
        PlannedSegment MainPathExitPlan = new PlannedSegment();
        schemeChoice = TryFindFittingSegment(SegmentCategory.Exit, MainPathStump);
        if (schemeChoice == null)
        {
            //Debug.Log("Could not plan exit part of main path");
            return false;
        }else
        {
            RegisterInGrid(schemeChoice, MainPathStump);
        }
        MainPathExitPlan.PlanFromScheme(schemeChoice, MainPathStump);
        dungeonPlan.AddSegment(MainPathExitPlan);


        //SIDEWAYS   
        PlannedSegment SidewayPlan;
        while (SidewayStumps.Count != 0) 
        {
            ChosenCategory = ChooseCategoryForStump(SidewayStumps[0]);
            SidewayPlan = new PlannedSegment();
            schemeChoice = TryFindFittingSegment(ChosenCategory, SidewayStumps[0]);
            if (schemeChoice == null)
            {
                //Debug.Log("Could not plan a sideway");
                return false;
            }else
            {
                RegisterInGrid(schemeChoice, SidewayStumps[0]);
            }
            SidewayPlan.PlanFromScheme(schemeChoice, SidewayStumps[0]);
            dungeonPlan.AddSegment(SidewayPlan);

            OpeningsStumps = SegmentStump.MakeStumpsForOpenings(SidewayPlan); //if sideway continues - add its exit stumps to the end of SidewayStumps
            foreach (SegmentStump stump in OpeningsStumps)
            {
                stump.StumpRole = DungeonSegmentRole.Sideway;
                SidewayStumps.Add(stump);
            }
            SidewayStumps.RemoveAt(0); //stump is built upon and should be removed
        }

        return true; //generated successfully
    }


    public override int ChooseCategoryForStump(SegmentStump stump)  //chooses category of future segment from this stump based on role, weight and chances in settings
    {
        int ExcessiveWeight;
        int ChanceToClose;
        bool Closing;
        bool Branching;

        switch (stump.StumpRole)
        {
            case DungeonSegmentRole.MainPath:   //main path
                ExcessiveWeight = stump.PathWeight - Settings.MainPathWeightThreshold;
                ChanceToClose = ExcessiveWeight * Settings.MainClosingChancePerExW;
                Closing = (ChanceToClose > 0) && (RandomZeroToInc(100) < ChanceToClose);
                Branching = RandomZeroToInc(100) < Settings.MainPathBranchingChance;
                if (Closing)
                {
                    return SegmentCategory.Exit;    //main closing is dungeon exit
                }
                else
                {
                    if (Branching) 
                        return SegmentCategory.Branch;  //main path is branching
                    return SegmentCategory.Corridor; // if not then a main corridor
                }
                //break;
            default:    //some kind of secondary branching
                ExcessiveWeight = stump.PathWeight - Settings.SidewaysWeightThreshold;
                ChanceToClose = ExcessiveWeight * Settings.SidewaysClosingChancePerExW;
                Closing = (ChanceToClose > 0) && (RandomZeroToInc(100) < ChanceToClose);
                Branching = RandomZeroToInc(100) < Settings.SidewaysBranchingChance;
                if (Closing)
                {
                    return SegmentCategory.Deadend;    //branch closing is plain deadend
                }
                else
                {
                    if (Branching)
                        return SegmentCategory.Branch;  //branch is branching further
                    return SegmentCategory.Corridor; // if not then a branch corridor
                }
                //break;

        }
    }
}
