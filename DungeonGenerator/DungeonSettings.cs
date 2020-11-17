using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonSettings
{
    

    [Range(10,150)]
    public int MainPathWeightThreshold = 50;   //  after what weight (amount of cells) the main path chance to continue should start to decay
    //public float MainPathChanceDecay = 0.9f;    //  DISCARDED Decay = the 100% chance to continue is mult by this subsequently every new seg after threshold
    [Range(1, 100)]
    public int MainClosingChancePerExW = 5;     //  % Chance for next segment to be closing multiplied by excessive weight after threshold
    [Range(0, 30)]
    public int SidewaysWeightThreshold = 0;
    [Range(1, 100)]
    public int SidewaysClosingChancePerExW = 30;  //  % Chance for next segment to be closing multiplied by excessive weight after threshold
    //public float BranchesChanceDecay = 0.5f;
    [Range(0, 100)]
    public int MainPathBranchingChance = 30;    //  % chance of next main path segment to be a branch
    [Range(0, 100)]
    public int SidewaysBranchingChance = 20;    //  % chance of next branch segment to be another branch
    
    public int DungeonTheme = 1;        //
    public int PlanGridDimensions = 100;      // (Max Index) dimentions of planning grid
}
