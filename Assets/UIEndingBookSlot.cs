using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndingBookSlot : MonoBehaviour {

    public Button Slot;
    public Text EndingTitle;
    public Text EndingBackgroundTitle;
    public Button BuyButton;
    public UIPointValue BuyCostValue;
    public Text EndingComplete;
    public GridLayoutGroup EndingSlotGrid;

    private int EndingBuyCost = 0;
    private BackgroundData BackgroundData = null;
    private List<UIEndingSceneSlot> EndingSceneSlotList = new List<UIEndingSceneSlot>();

    void Awake()
    {
        Slot.onClick.AddListener(OnClickEnding);
        BuyButton.onClick.AddListener(OnClickEndingBuy);
    }

    public void SetData(int backgroundId)
    {
        for (int i = 0; i < EndingSceneSlotList.Count; i++)
        {
            DestroyImmediate(EndingSceneSlotList[i].gameObject);
        }


        BackgroundData = DataManager.Instance.BackGroundDataDic[backgroundId];
        EndingTitle.text = LocalizeData.Instance.GetLocalizeString(BackgroundData.endingTitle);
        EndingBackgroundTitle.text = LocalizeData.Instance.GetLocalizeString("POPUP_ENDING_SLOT_BG", LocalizeData.Instance.GetLocalizeString(BackgroundData.name));

        EndingBuyCost = 0;
        for (int i = 0; i < BackgroundData.endingList.Count; i++)
        {
            int id = BackgroundData.endingList[i];
            if (PlayerData.Instance.HasSkin(CommonData.SKIN_TYPE.ENDING, id))
                continue;

            var endingData = DataManager.Instance.EndingDataList[id];
            EndingBuyCost += endingData.cost;

            var obj = Instantiate(Resources.Load("Prefab/UIEndingSceneSlot"), EndingSlotGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIEndingSceneSlot>();
            EndingSceneSlotList.Add(slot);
            slot.SetData(endingData.id);
        }

        if(EndingBuyCost > 0)
        {
            BuyCostValue.gameObject.SetActive(true);
            BuyCostValue.SetValue(EndingBuyCost, LocalizeData.Instance.GetLocalizeString("COMMON_BUY"));
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

        // 엔딩 구매 팝업
    }

    public void OnClickEnding()
    {
        // 엔딩 팝업
    }
}
