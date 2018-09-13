using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        public UnityAction EndAction = null;

        public PopupData(int endingGroupId, int endingId = 0, UnityAction endAction = null)
        {
            EndingGroupId = endingGroupId;
            EndingId = endingId;
            EndAction = endAction;
        }
    }
    public Text Title;
    public GameObject LockObj;
    public Image EndingScene;
    public GameObject DescObj;
    public Text Desc;
    public Button Prev;
    public Button OK;
    public Button Next;
    public Button Buy;
    public UIPointValue BuyCost;

    private int EndingGroupId;
    private int EndingId;
    private List<EndingData> EndingSceneList = new List<EndingData>();
    private int SelectIndex = 0;
    private UnityAction EndAction = null;

    public void Awake()
    {
        Prev.onClick.AddListener(OnClickPrev);
        OK.onClick.AddListener(OnClickOk);
        Next.onClick.AddListener(OnClickNext);
        Buy.onClick.AddListener(OnClickEndingBuy);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;
        EndingGroupId = popupData.EndingGroupId;
        EndingId = popupData.EndingId;
        EndAction = popupData.EndAction;
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
            DescObj.gameObject.SetActive(false);
            Buy.gameObject.SetActive(false);

            CommonFunc.SetImageFile(EndingSceneList[SelectIndex].img, ref EndingScene, false);
        }
        else
        {
            if(SelectIndex == 0 || PlayerData.Instance.HasEnding(EndingSceneList[SelectIndex - 1].id))
            {
                Buy.gameObject.SetActive(true);
                BuyCost.SetValue(EndingSceneList[SelectIndex].cost);
            }
            else
                Buy.gameObject.SetActive(false);

            LockObj.gameObject.SetActive(true);
            EndingScene.gameObject.SetActive(false);
            DescObj.gameObject.SetActive(true);

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

    public void OnClickEndingBuy()
    {
        UnityAction yesAction = () =>
        {
            if (CommonFunc.UseCoin(EndingSceneList[SelectIndex].cost))
            {
                PlayerData.Instance.AddEnding(EndingSceneList[SelectIndex].id);
                SetEndingScene();
                if (EndAction != null)
                    EndAction();
            }
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ENDING_TITLE"), yesAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }
}
