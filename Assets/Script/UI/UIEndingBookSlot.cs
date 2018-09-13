using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIEndingBookSlot : MonoBehaviour {

    public Button Slot;
    public GameObject EndingObj;
    public Text EndingTitle;
    public Button BuyButton;
    public UIPointValue BuyCostValue;
    public Text EndingComplete;
    public GridLayoutGroup EndingSlotGrid;

    public Text UpdateDesc;

    private int EndingBuyCost = 0;
    private EndingGroupData EndingGroupData = null;
    private List<UIEndingSceneSlot> EndingSceneSlotList = new List<UIEndingSceneSlot>();

    void Awake()
    {
        Slot.onClick.AddListener(OnClickEnding);
        BuyButton.onClick.AddListener(OnClickEndingBuy);
    }

    public void SetData(int backgroundId, int endingGroupId)
    {
        for (int i = 0; i < EndingSceneSlotList.Count; i++)
        {
            DestroyImmediate(EndingSceneSlotList[i].gameObject);
        }
        EndingSceneSlotList.Clear();

        if(endingGroupId == 0)
        {
            EndingGroupData = null;
            EndingObj.gameObject.SetActive(false);
            UpdateDesc.gameObject.SetActive(true);
            return;
        }
        else
        {
            EndingObj.gameObject.SetActive(true);
            UpdateDesc.gameObject.SetActive(false);
        }


        EndingGroupData = DataManager.Instance.EndingGroupDataList[endingGroupId];
        EndingTitle.text = LocalizeData.Instance.GetLocalizeString(EndingGroupData.name);
        var backgroudData = DataManager.Instance.BackGroundDataDic[backgroundId];

        EndingBuyCost = 0;
        for (int i = 0; i < EndingGroupData.ending_list.Count; i++)
        {
            int groupId = EndingGroupData.id;
            int id = EndingGroupData.ending_list[i];
            var endingData = DataManager.Instance.EndingDataList[id];
            if (PlayerData.Instance.HasEnding(id) == false)
                EndingBuyCost += endingData.cost;

            var obj = Instantiate(Resources.Load("Prefab/UIEndingSceneSlot"), EndingSlotGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIEndingSceneSlot>();
            EndingSceneSlotList.Add(slot);
            slot.SlotButton.onClick.AddListener(() => { OnClickEnding(groupId, id); });
            slot.SetData(EndingGroupData.id, endingData.id);
        }

        if(EndingBuyCost > 0)
        {
            BuyCostValue.gameObject.SetActive(true);
            BuyCostValue.SetValue(EndingBuyCost);
            EndingComplete.text = "";
        }
        else
        {
            BuyCostValue.gameObject.SetActive(false);
            EndingComplete.text = LocalizeData.Instance.GetLocalizeString("POPUP_ENDING_SLOT_ALL_HAVE");
        }
    }

    public void OnClickEndingBuy()
    {
        if (EndingBuyCost <= 0)
            return;

        UnityAction yesAction = () =>
        {
            if (CommonFunc.UseCoin(EndingBuyCost))
            {
                for (int i = 0; i < EndingGroupData.ending_list.Count; i++)
                {
                    PlayerData.Instance.AddEnding(EndingGroupData.ending_list[i]);
                }
                BuyCostValue.gameObject.SetActive(false);
                EndingComplete.text = LocalizeData.Instance.GetLocalizeString("POPUP_ENDING_SLOT_ALL_HAVE");
                Refresh();
            }
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ENDING_TITLE"), yesAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }

    public void OnClickEnding()
    {
        if (EndingGroupData == null)
            return;

        var data = new PopupGameEndingScene.PopupData(EndingGroupData.id, EndingGroupData.ending_list[0], Refresh);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_ENDING_SCENE, data);
    }

    public void Refresh()
    {
        for (int i = 0; i < EndingGroupData.ending_list.Count; i++)
        {
            int groupId = EndingGroupData.id;
            int id = EndingGroupData.ending_list[i];
            EndingSceneSlotList[i].SlotButton.onClick.AddListener(() => { OnClickEnding(groupId, id); });
            EndingSceneSlotList[i].SetData(EndingGroupData.id, EndingGroupData.ending_list[i]);
        }
    }

    public void OnClickEnding(int endingGroupId, int endingId)
    {
        var data = new PopupGameEndingScene.PopupData(endingGroupId, endingId, Refresh);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_ENDING_SCENE, data);
    }
}
