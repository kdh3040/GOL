using System.Collections;
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

    public enum SKIN_TYPE
    {
        NONE,
        CHAR,
        DOOR,
        ENDDING, // 사용 하지 않음
        BACKGROUND,
    }

    public enum POINT_TYPE
    {
        CASH,
        DDONG,
        COIN,
    }

    public static int SHIELD_ITEM_ID = 2;
    public static int NOTE_GROUP_NOTE_COUNT = 3;
    public static float NOTE_GROUP_INTERVAL = 4.0f;
    public static int MAX_DDONG_COUNT = 5;
    public static float DDONG_VIEW_INTERVAL = 5f;
    public static string[] NOTE_DELETE_MSG = { "Renewal/UI/icon_lock" };
}
