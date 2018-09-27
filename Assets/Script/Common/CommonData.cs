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
        BACKGROUND,
    }

    public enum POINT_TYPE
    {
        CASH,
        DDONG,
        COIN,
    }

    public enum SOUND_TYPE
    {
        BUTTON,
        DOOR,
        BUY,
        LEVEL,
        GAME_END,
        EQUIP,
        GAME_START,
        GAME_PLAY,
        REVIVAL,
        NEW_RECORD,
        DOOR_CLICK,
        DOOR_METAL,
        DOOR_DESERT,
        DOOR_SPACE_1,
        DOOR_SPACE_2

    }

    public enum DOOR_EFFECT_SOUND_TYPE
    {
        SHIELD,
        IRONDOOR,
    }

    public static int SHIELD_ITEM_ID = 2;
    public static int NOTE_GROUP_NOTE_COUNT = 3;
    public static float NOTE_GROUP_INTERVAL = 3.0f;
    public static int MAX_DDONG_COUNT = 10;
    public static float DDONG_VIEW_INTERVAL = 5f;
    public static string[] PURCHASE_ID_ARRAY = { "coin_1", "coin_2", "coin_3", "coin_4", "coin_5", "coin_6", "coin_7" };
    public static string[] PURCHASE_DDONG_ARRAY = { "ddong_1", "ddong_2", "ddong_3", "ddong_4", "ddong_5", "ddong_6", "ddong_7" };
    public static string[] NOTE_DELETE_MSG = { "Renewal/UI/text_door", "Renewal/UI/text_smell", "Renewal/UI/text_what", "Renewal/UI/text_who" };
    public static int GAME_CONTINUE_MAX_COUNT = 1;
}
