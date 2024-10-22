﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ArchivedSegment
{    
    public List<SegmentCell>[] Occupation = new List<SegmentCell>[SegmentRotation.PossibleRotations]; //all cells that segment fully or partly occupies  [] - rotated variants
    public List<SegmentCellOriented>[] Openings = new List<SegmentCellOriented>[SegmentRotation.PossibleRotations]; // for each exit: cell that an exit leads into; place for new segment going out of this one
    public int EntranceDoorType;  //type of segment entrance door at local (0,0) facing down
    public int Theme;       
    public int Category;    // Deadend / Corridor / Branch / Entrance / Exit
    public string PrefabName; 
    
    public int Chance; // (WIP) chance weight of being chosen 20-max  10-default  1-most rare
    
    //autogenerated
    public int DoorsCombined = 0; // doors that are present somewhere in this segment    A bit mask, amalgam of all door types

    public List<ArchivedSegment> EDVariants; //Entrance-Displaced Variants (Generated variants of segment with entrance for each of its exits)
    public int Weight; // how "heavy" is the segment (number of cells it occupies)
    public SegmentCellOriented[] PivotCell = new SegmentCellOriented[SegmentRotation.PossibleRotations];  // (local grid) position of prefab pivot. Facing describes the angle of orthonormed rotation

    public ArchivedSegment( List<SegmentCell> occupation,
                            List<SegmentCellOriented> openings,
                            int entranceDoorType,
                            int theme,
                            int category,
                            string prefabName,
                            int chance = 10,
                            bool original = true) //false - if it is not entered manualy in codex and instead is an autogenerated entrance-displaced subvariant
    {
        Occupation[SegmentRotation.Up] = occupation;
        Openings[SegmentRotation.Up] = openings;
        EntranceDoorType = entranceDoorType;
        Theme = theme;
        Category = category;
        PrefabName = prefabName;
        Chance = chance;
        PivotCell[SegmentRotation.Up] = new SegmentCellOriented(0, 0, SegmentRotation.Up, DoorType.NonExistant, 0);
        Weight = Occupation[SegmentRotation.Up].Count;
        MakeDoorMask();
        if (original)
        {
            FillRotations(this);
            EDVariants = new List<ArchivedSegment>();
            Variate();
        }
    }

    public void Variate() //make entrance-displaced variants and fill in their rotations
    {
        if ( (Openings[SegmentRotation.Up].Count != 0) && (Category != SegmentCategory.Entrance)) //entrance can not have alternative way to enter in it
        {
            List<SegmentCell> NewOccupation;       
            List<SegmentCellOriented> NewOpenings;
            int NeededSegmentRotation = 0;
            int UCorrection = 0;
            int RCorrection = 0;
            foreach (SegmentCellOriented opening in Openings[SegmentRotation.Up])
            {
                NewOccupation = new List<SegmentCell>();
                NewOpenings = new List<SegmentCellOriented>();
                switch (opening.Facing)
                {
                    case CellFacing.Up:
                        NeededSegmentRotation = SegmentRotation.Down;         //rotation of the original that will be a default(up) rotation for variant
                        UCorrection = opening.U - 1;    //movement of the segment after rotation that is needed for the cell with opening(new entrance) to be at local 0.0   
                        RCorrection = opening.R;
                        break;
                    case CellFacing.Right:
                        NeededSegmentRotation = SegmentRotation.Right; 
                        UCorrection = opening.R - 1; 
                        RCorrection = - opening.U; 
                        break;
                    case CellFacing.Down:
                        NeededSegmentRotation = SegmentRotation.Up;
                        UCorrection = - opening.U - 1;
                        RCorrection = - opening.R;
                        break;
                    case CellFacing.Left:
                        NeededSegmentRotation = SegmentRotation.Left;
                        UCorrection = - opening.R - 1;
                        RCorrection = opening.U;
                        break;
                }

                foreach (SegmentCell rotcell in Occupation[NeededSegmentRotation])
                {
                    NewOccupation.Add(new SegmentCell(rotcell.U + UCorrection, rotcell.R + RCorrection));
                }
 
                foreach(SegmentCellOriented rotopening in Openings[NeededSegmentRotation])
                {
                    if( (rotopening.U + UCorrection == -1) && (rotopening.R + RCorrection == 0) && (rotopening.Facing == CellFacing.Down) ) //check and skip if exit with current opening is the new entrance
                        continue;
                    NewOpenings.Add( new SegmentCellOriented(rotopening.U + UCorrection, rotopening.R + RCorrection, rotopening.Facing, rotopening.Type, rotopening.Altitude - opening.Altitude ) );
                }

                int OldEntranceOpeningU = 0;    //Old entrance should be listed as viable opening now
                int OldEntranceOpeningR = 0;
                int OldEntranceOpeningFacing = 0;
                switch (NeededSegmentRotation)
                {
                    case SegmentRotation.Up:
                        OldEntranceOpeningU = UCorrection - 1;
                        OldEntranceOpeningR = RCorrection;
                        OldEntranceOpeningFacing = CellFacing.Down;
                        break;
                    case SegmentRotation.Right:
                        OldEntranceOpeningU = UCorrection;
                        OldEntranceOpeningR = RCorrection - 1;
                        OldEntranceOpeningFacing = CellFacing.Left;
                        break;
                    case SegmentRotation.Down:
                        OldEntranceOpeningU = UCorrection + 1;
                        OldEntranceOpeningR = RCorrection;
                        OldEntranceOpeningFacing = CellFacing.Up;
                        break;
                    case SegmentRotation.Left:
                        OldEntranceOpeningU = UCorrection;
                        OldEntranceOpeningR = RCorrection + 1;
                        OldEntranceOpeningFacing = CellFacing.Right;
                        break;
                }
                NewOpenings.Add( new SegmentCellOriented(OldEntranceOpeningU, OldEntranceOpeningR, OldEntranceOpeningFacing, EntranceDoorType, - opening.Altitude) );  //add an old entrance as possible exit with opening

                ArchivedSegment Variant = new ArchivedSegment(  NewOccupation,
                                                                NewOpenings,
                                                                opening.Type,
                                                                Theme,
                                                                Category,
                                                                PrefabName,
                                                                Chance,
                                                                false);
                //pivot of the prefab is displaced and has added rotation
                Variant.PivotCell[SegmentRotation.Up] = new SegmentCellOriented( UCorrection, RCorrection, NeededSegmentRotation, DoorType.NonExistant, -opening.Altitude);
                FillRotations(Variant);
                EDVariants.Add(Variant);
            }
        }
    }

    public void MakeDoorMask() // create bit mask for segment, amalgam of all door types present somewhere in it
    {
        DoorsCombined = DoorsCombined | EntranceDoorType;
        if(Openings[SegmentRotation.Up].Count != 0)
        {
            foreach(SegmentCellOriented opening in Openings[SegmentRotation.Up])
            {
                DoorsCombined = DoorsCombined | opening.Type;
            }
        }
    }

    public void FillRotations(ArchivedSegment segment) // fill in the 90 180 270 rotated variants of segment
    {
        //Pivot position and rotation (matters when segment is an ED variant and prefab pivot is displaced and has it's own rotation)
        segment.PivotCell[SegmentRotation.Right] = new SegmentCellOriented( -segment.PivotCell[SegmentRotation.Up].R,  
                                                                            segment.PivotCell[SegmentRotation.Up].U,  
                                                                            (segment.PivotCell[SegmentRotation.Up].Facing+1)%4,  
                                                                            0,  
                                                                            segment.PivotCell[SegmentRotation.Up].Altitude );
        segment.PivotCell[SegmentRotation.Down] = new SegmentCellOriented(  -segment.PivotCell[SegmentRotation.Up].U,  
                                                                            -segment.PivotCell[SegmentRotation.Up].R,  
                                                                            (segment.PivotCell[SegmentRotation.Up].Facing+2)%4,  
                                                                            0,  
                                                                            segment.PivotCell[SegmentRotation.Up].Altitude );
        segment.PivotCell[SegmentRotation.Left] = new SegmentCellOriented( segment.PivotCell[SegmentRotation.Up].R,  
                                                                            -segment.PivotCell[SegmentRotation.Up].U,  
                                                                            (segment.PivotCell[SegmentRotation.Up].Facing+3)%4,  
                                                                            0,  
                                                                            segment.PivotCell[SegmentRotation.Up].Altitude );
        
        segment.Occupation[SegmentRotation.Right] = new List<SegmentCell>();
        segment.Occupation[SegmentRotation.Down] = new List<SegmentCell>();
        segment.Occupation[SegmentRotation.Left] = new List<SegmentCell>();
        foreach (SegmentCell cell in segment.Occupation[SegmentRotation.Up])
        {
            segment.Occupation[SegmentRotation.Right].Add(new SegmentCell(-cell.R ,  cell.U));
            segment.Occupation[SegmentRotation.Down].Add(new SegmentCell(-cell.U , -cell.R));
            segment.Occupation[SegmentRotation.Left].Add(new SegmentCell( cell.R , -cell.U));
        }

        segment.Openings[SegmentRotation.Right] = new List<SegmentCellOriented>();
        segment.Openings[SegmentRotation.Down] = new List<SegmentCellOriented>();
        segment.Openings[SegmentRotation.Left] = new List<SegmentCellOriented>();
        foreach(SegmentCellOriented opening in segment.Openings[SegmentRotation.Up])
        {
            segment.Openings[SegmentRotation.Right].Add(new SegmentCellOriented(-opening.R , opening.U , (opening.Facing+1)%4 , opening.Type , opening.Altitude ));
            segment.Openings[SegmentRotation.Down].Add(new SegmentCellOriented(-opening.U , -opening.R , (opening.Facing+2)%4 , opening.Type , opening.Altitude ));
            segment.Openings[SegmentRotation.Left].Add(new SegmentCellOriented(opening.R , -opening.U , (opening.Facing+3)%4 , opening.Type , opening.Altitude ));
        }
    }
}
