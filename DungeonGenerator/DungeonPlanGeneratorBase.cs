using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DungeonPlanGeneratorBase
{
    public byte[,] PlanningGrid;
    public DungeonSettings Settings = new DungeonSettings();
    //public int PathRetriesAllowed; //how many tries of building a full main path  OR  all branches at once is allowed untill Failed
    public int SegmentRetriesAllowed = 20; //how many tries of finding an appropriate segment(with all its ED subvariants) is allowed untill Failed
    public bool UseBranchingEntropy = false;
    public Vector2 EntranceMargin = new Vector2(0.3f, 0.5f);    // multiplier-coords in planning grid for dungeon entrance. (0,0) is bottom left corner. (0.5,0.5) is center. (1,1) is top right

    //Main Generator
    public abstract bool GeneratePlan(out DungeonPlan dungeonPlan);

    //Building helper methods
    public abstract int ChooseCategoryForStump(SegmentStump stump);  //chooses category of future segment from this stump based on weight and chances in settings


    public virtual ArchivedSegment TryFindFittingSegment(int category, SegmentStump whereTo)
    {
        int FailCounter = 0;
        bool FitCheck, DoorCheck;
        ArchivedSegment choice;
        List<ArchivedSegment> Variants = new List<ArchivedSegment>();
        while (FailCounter < SegmentRetriesAllowed)
        {
            Variants.Clear();
            choice = SegmentCodex.GetRandomSegment(category, Settings.DungeonTheme, whereTo.ZeroPosRot.Type);
            Variants.Add(choice);
            if ((choice.EDVariants != null))
                Variants.AddRange(choice.EDVariants);   // segment and all its entrance-displaced variants
            ShuffleList<ArchivedSegment>(Variants);

            foreach (ArchivedSegment variant in Variants)
            {
                DoorCheck = (whereTo.ZeroPosRot.Type == DoorType.NonExistant || DoorType.AreTheeseDoorsCompatible(variant.EntranceDoorType, whereTo.ZeroPosRot.Type));
                if (DoorCheck)
                {
                    FitCheck = CanItFit(variant, whereTo);
                    if (FitCheck)
                    {
                        return variant;
                    }
                }
            }
            FailCounter++;
        }
        return null;
    }


    public virtual bool CanItFit(ArchivedSegment what, SegmentStump whereTo)
    {
        int gridU, gridR;
        foreach (SegmentCell cell in what.Occupation[whereTo.ZeroPosRot.Facing])    //main cells intersection check
        {
            gridU = whereTo.ZeroPosRot.U + cell.U;
            gridR = whereTo.ZeroPosRot.R + cell.R;
            if ((gridU < 0) || (gridU > Settings.PlanGridDimensions) || (gridR < 0) || (gridR > Settings.PlanGridDimensions))
                return false;
            
            if( (cell.U == 0) && (cell.R == 0) ) //is it an entrance cell
            {
                if (PlanningGrid[gridU, gridR] == GridOccupation.BlockedByCell) //entrance can be planned on top of openings(that's the point)
                {
                    return false;
                }
            }
            else
            {
                if (PlanningGrid[gridU, gridR] != GridOccupation.Empty) //not-entrance can be planned only on empty space
                {
                    return false;
                }
            }
        }

        foreach (SegmentCellOriented opening in what.Openings[whereTo.ZeroPosRot.Facing])    //openings intersection check
        {
            gridU = whereTo.ZeroPosRot.U + opening.U;
            gridR = whereTo.ZeroPosRot.R + opening.R;
            if ((gridU < 0) || (gridU > Settings.PlanGridDimensions) || (gridR < 0) || (gridR > Settings.PlanGridDimensions))
                return false;
            if (PlanningGrid[gridU, gridR] != GridOccupation.Empty) //opening for future segment needs at least 1 cell of open space
            {
                return false;
            }
        }
        return true;
    }

    public virtual void RegisterInGrid(ArchivedSegment what, SegmentStump where)
    {
        foreach (SegmentCell cell in what.Occupation[where.ZeroPosRot.Facing])
        {
            PlanningGrid[where.ZeroPosRot.U + cell.U, where.ZeroPosRot.R + cell.R] = GridOccupation.BlockedByCell;
        }
        foreach (SegmentCellOriented opening in what.Openings[where.ZeroPosRot.Facing])
        {
            PlanningGrid[where.ZeroPosRot.U + opening.U, where.ZeroPosRot.R + opening.R] = GridOccupation.BlockedByOpening;
        }
    }


    public void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        int k;
        while (n > 1)
        {
            n--;
            k = RandomZeroToInc(n);
            T tmp = list[k];
            list[k] = list[n];
            list[n] = tmp;
        }
    }

    public int RandomZeroToInc(int incMax)
    {
        return Random.Range(0, incMax + 1);
    }
}
