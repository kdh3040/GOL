using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndingSceneSlot : MonoBehaviour {
    public GameObject LockObj;
    public Image EnddingScene;

    public void SetData(int endingId)
    {
        var data = DataManager.Instance.EndingDataList[endingId];
        if (PlayerData.Instance.HasSkin(CommonData.SKIN_TYPE.ENDING, endingId))
        {
            LockObj.gameObject.SetActive(false);
            EnddingScene.gameObject.SetActive(true);
            CommonFunc.SetImageFile(data.img, ref EnddingScene);
        }
        else
        {
            LockObj.gameObject.SetActive(true);
            EnddingScene.gameObject.SetActive(false);
        }
    }
}
