using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndingSceneSlot : MonoBehaviour {
    public Button SlotButton;
    public GameObject LockObj;
    public Image EnddingScene;

    [System.NonSerialized]
    private int EndingGroupId;
    [System.NonSerialized]
    private int EndingId;

    public void SetData(int endingGroupId, int endingId)
    {
        EndingGroupId = endingGroupId;
        EndingId = endingId;
        var data = DataManager.Instance.EndingDataList[endingId];
        if (PlayerData.Instance.HasEnding(endingId))
        {
            LockObj.gameObject.SetActive(false);
            EnddingScene.gameObject.SetActive(true);
            CommonFunc.SetImageFile(data.img, ref EnddingScene, false);
        }
        else
        {
            LockObj.gameObject.SetActive(true);
            EnddingScene.gameObject.SetActive(false);
        }
    }
}
