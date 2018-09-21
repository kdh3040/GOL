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
        public bool NewOpen = false;
        public PopupData(int endingGroupId, int endingId = 0, UnityAction endAction = null, bool newOpen = false)
        {
            EndingGroupId = endingGroupId;
            EndingId = endingId;
            EndAction = endAction;
            NewOpen = newOpen;
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
    private bool NewOpenEnding = false;

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
        NewOpenEnding = popupData.NewOpen;
        SelectIndex = 0;

        var groupData = DataManager.Instance.EndingGroupDataList[EndingGroupId];
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

        var endingData = EndingSceneList[SelectIndex];
        if (NewOpenEnding)
            Title.text = LocalizeData.Instance.GetLocalizeString("ENDING_OPEN_TITLE", LocalizeData.Instance.GetLocalizeString(endingData.name));
        else
            Title.text = LocalizeData.Instance.GetLocalizeString(endingData.name);

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
                SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUY);
                PlayerData.Instance.AddEnding(EndingSceneList[SelectIndex].id);
                SetEndingScene();
                if (EndAction != null)
                    EndAction();
            }
        };

        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ENDING_TITLE"),
            LocalizeData.Instance.GetLocalizeString("COMMON_BUY"),
            PopupMsg.MSG_POPUP_TYPE.BUY_NO,
            CommonData.POINT_TYPE.COIN,
            EndingSceneList[SelectIndex].cost,
            yesAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }
   
}
