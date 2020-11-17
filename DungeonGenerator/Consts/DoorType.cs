using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DoorType 
{
    public const int NonExistant = 0;
    public const int Generic = 1;
    //public const int Wide = 2;  //WIP
    //public const int Tight = 4;
    //public const int DisplacedUpToLeft = 8;

    public const int DoorTypesAmount = 2;

    


    public static int GetCompatMaskForDoorType(int doorType) // returns mask with bits for compatible door type with this door
    {
        switch (doorType)
        {
            case 1:
                return 1 | 2;
            case 2:
                return 2 | 1;
            /*case 4:
                return 8 | 32;  //WIP
            case 8:
                return 4 | 16;
            case 16:
                return 32 | 8;
            case 32:
                return 16 | 4;*/
        }
        return 0;
    }

    public static bool AreTheeseDoorsCompatible(int door1Type, int door2Type)
    {
        return ((GetCompatMaskForDoorType(door1Type) & door2Type) != 0);
    }
}
