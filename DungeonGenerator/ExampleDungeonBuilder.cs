using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleDungeonBuilder : MonoBehaviour
{
    //[Header("Metrics")]
    public int MaxGenerateAttempts = 200;
    public Vector3 WorldPosOfZero = new Vector3(0f, 1, 0f);     //world coordinates - position of (0,0) of grid
    public const float SegmentCellSize = 20; //Unity meters per segment cell (planning grid cell)
    public DungeonSettings Settings = new DungeonSettings();
    public DungeonPlan ThePlan;
    public DungeonPlanGeneratorBase PlanGenerator;
    

    void Start()
    {
        PlanGenerator = new DungeonPlanGeneratorLinear();
        PlanGenerator.Settings = Settings;
        PlanGenerator.SegmentRetriesAllowed = 20;   //Default=20    - max attempts to find a different suitable segment in codex            
        PlanGenerator.UseBranchingEntropy = false;  //Default=false - WIP - generator can postpone the sideway's further growth or 
                                                                    //branching if dungeon topology cant allow it in given place for given sideway
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Cleaning up
            for (int i = 0; i < transform.childCount; ++i) 
            { 
                Destroy(transform.GetChild(i).gameObject); 
            }
            //Generating plan
            int Attempts = 0;
            bool Success;
            do
            {
                Success = PlanGenerator.GeneratePlan(out ThePlan);
                Attempts++;
            }while(Success == false && Attempts < MaxGenerateAttempts);

            if(Success)
            {
                Debug.Log("Plan generated successfully. Attempts needed: " + Attempts);
            }
            else
            {
                Debug.Log("Could not generate plan. Max attempts reached: " + Attempts);
                return;
            }

            //Building dungeon
            foreach (PlannedSegment seg in ThePlan.AllSegments)
            {
                Vector3 WorldPos = new Vector3( WorldPosOfZero.x + ( seg.ZeroPosRot.R + seg.Scheme.PivotCell[seg.ZeroPosRot.Facing].R ) * SegmentCellSize,
                                                WorldPosOfZero.y + (seg.ZeroPosRot.Altitude + seg.Scheme.PivotCell[seg.ZeroPosRot.Facing].Altitude) * SegmentCellSize,
                                                WorldPosOfZero.z + ( seg.ZeroPosRot.U + seg.Scheme.PivotCell[seg.ZeroPosRot.Facing].U ) * SegmentCellSize);

                Instantiate(Resources.Load("Segments/" + seg.Scheme.PrefabName) as GameObject,
                            WorldPos,
                            Quaternion.identity * Quaternion.AngleAxis( 90 * seg.Scheme.PivotCell[seg.ZeroPosRot.Facing].Facing, Vector3.up),
                            gameObject.transform);
            }

        }
        
            


    }
}
