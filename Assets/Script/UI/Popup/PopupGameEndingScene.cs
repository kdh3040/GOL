using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupGameEndingScene : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_ENDING_SCENE;
    }

    public class PopupData : PopupUIData
    {
        public int EndingGroupId;
        public int EndingId;
        public PopupData(int endingGroupId, int endingId = 0)
        {
            EndingGroupId = endingGroupId;
            EndingId = endingId;
        }
    }
    public Text Title;
    public GameObject LockObj;
    public Image EndingScene;
    public Text Desc;
    public Button Prev;
    public Button OK;
    public Button Next;

    private int EndingGroupId;
    private int EndingId;
    private List<EndingData> EndingSceneList = new List<EndingData>();
    private int SelectIndex = 0;

    public void Awake()
    {
        Prev.onClick.AddListener(OnClickPrev);
        OK.onClick.AddListener(OnClickOk);
        Next.onClick.AddListener(OnClickNext);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        EndingGroupId = popupData.EndingGroupId;
        EndingId = popupData.EndingId;
        SelectIndex = 0;

        var groupData = DataManager.Instance.EndingGroupDataList[EndingGroupId];
        Title.text = LocalizeData.Instance.GetLocalizeString(groupData.name);

        EndingSceneList.Clear();
        for (int i = 0; i < groupData.ending_list.Count; i++)
        {
            var enddingData = DataManager.Instance.EndingDataList[groupData.ending_list[i]];
            EndingSceneList.Add(enddingData);

            if (EndingId != 0 && EndingId == enddingData.id)
                SelectIndex = i;
        }

        SetEndingScene();
    }

    public void SetEndingScene()
    {
        if(PlayerData.Instance.HasEnding(EndingSceneList[SelectIndex].id))
        {
            LockObj.gameObject.SetActive(false);
            EndingScene.gameObject.SetActive(true);

            Desc.text = EndingSceneList[SelectIndex].GetLocalizeDesc();
            CommonFunc.SetImageFile(EndingSceneList[SelectIndex].img, ref EndingScene, false);
        }
        else
        {
            LockObj.gameObject.SetActive(true);
            EndingScene.gameObject.SetActive(false);

            Desc.text = EndingSceneList[SelectIndex].GetConditionDesc();
        }

        RefreshButtons();
    }

    public void RefreshButtons()
    {
        Prev.gameObject.SetActive(SelectIndex != 0);
        Next.gameObject.SetActive(SelectIndex < EndingSceneList.Count - 1);
    }

    public void OnClickOk()
    {
        PopupManager.Instance.DismissPopup();
    }

    public void OnClickPrev()
    {
        SelectIndex--;
        if (SelectIndex < 0)
            SelectIndex = 0;

        SetEndingScene();
    }

    public void OnClickNext()
    {
        SelectIndex++;

        if (SelectIndex >= EndingSceneList.Count)
            SelectIndex = EndingSceneList.Count - 1;

        SetEndingScene();
    }
}
