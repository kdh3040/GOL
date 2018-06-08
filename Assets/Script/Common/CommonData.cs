using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData : MonoBehaviour {
    public enum NOTE_POS_TYPE
    {
        NONE,
        INDEX_1,
        INDEX_2,
        INDEX_3,
        MAX
    }

    public static float COMBO_KEEP_TIME = 2f;

    public static string STRING_COMBO_COUNT = "COMBO : {0}";
    public static string STRING_SCORE_COUNT = "점수 : {0}";
}
