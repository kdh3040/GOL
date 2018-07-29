using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData : MonoBehaviour {
    public enum NOTE_LINE
    {
        NONE,
        INDEX_1,
        INDEX_2,
        INDEX_3,
        MAX
    }

    public enum NOTE_TYPE
    {
        NONE,
        NORMAL,
        ITEM
    }

    public enum ITEM_SLOT_TYPE
    {
        NORMAL,
        SHIELD,
    }

    public enum ITEM_SLOT_INDEX
    {
        LEFT = 0,
        RIGHT = 1
    }

    public enum SKIN_TYPE
    {
        NONE,
        CHAR,
        DOOR,
        ENDING,
        BACKGROUND,
    }

    public static int SHIELD_ITEM_ID = 2;
    public static int NOTE_GROUP_NOTE_COUNT = 3;
}
