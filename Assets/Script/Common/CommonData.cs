﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData : MonoBehaviour {
    public enum NOTE_LINE
    {
        INDEX_1,
        INDEX_2,
        INDEX_3,
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
        RIGHT = 1,
        SHIELD = 2
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
    public static float NOTE_GROUP_INTERVAL = 3.0f;
    public static float NOTE_TOUCH_DELETE_INTERVAL = 2.0f;
    public static int MAX_DDONG_COUNT = 5;
}
