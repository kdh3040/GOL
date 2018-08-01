using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupGameShopSkin : MonoBehaviour {

    public Image SelectSkinIcon;
    public Text SelectSkinDesc;
    public Button SkinBuy;
    public Button SkinEquip;
    public GridLayoutGroup SkinGrid;
    public ScrollRect ScrollRect;

    private List<UIShopSkinSlot> SkinList = new List<UIShopSkinSlot>();
    private int SelectSkinId = 0;
    private CommonData.SKIN_TYPE SelectSkinType = CommonData.SKIN_TYPE.NONE;
    private Dictionary<CommonData.SKIN_TYPE, List<int>> SkinIndexDic = new Dictionary<CommonData.SKIN_TYPE, List<int>>();

    void Awake()
    {
        SkinBuy.onClick.AddListener(OnClickBuy);
        SkinEquip.onClick.AddListener(OnClickEquip);
    }

    public CommonData.SKIN_TYPE ConvertShopTab(PopupGameShop.TAB_TYPE type)
    {
        switch (type)
        {
            case PopupGameShop.TAB_TYPE.CHAR:
                return CommonData.SKIN_TYPE.CHAR;
            case PopupGameShop.TAB_TYPE.DOOR:
                return CommonData.SKIN_TYPE.DOOR;
            case PopupGameShop.TAB_TYPE.BG:
                return CommonData.SKIN_TYPE.BACKGROUND;
            default:
                break;
        }

        return CommonData.SKIN_TYPE.NONE;
    }

    public void ShowUI(PopupGameShop.TAB_TYPE type)
    {
        if (SelectSkinType == ConvertShopTab(type))
            return;

        SelectSkinType = ConvertShopTab(type);
        if (SkinIndexDic.Count <= 0)
        {
            var tempCharIndexList = new List<int>();
            var charDataDicEnumerator = DataManager.Instance.CharDataDic.GetEnumerator();
            while (charDataDicEnumerator.MoveNext())
            {
                tempCharIndexList.Add(charDataDicEnumerator.Current.Key);
            }

            tempCharIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            SkinIndexDic.Add(CommonData.SKIN_TYPE.CHAR, tempCharIndexList);

            var tempDoorIndexList = new List<int>();
            var doorDataDicEnumerator = DataManager.Instance.DoorDataDic.GetEnumerator();
            while (doorDataDicEnumerator.MoveNext())
            {
                tempDoorIndexList.Add(doorDataDicEnumerator.Current.Key);
            }

            tempDoorIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            SkinIndexDic.Add(CommonData.SKIN_TYPE.DOOR, tempDoorIndexList);

            var tempBGIndexList = new List<int>();
            var bgDataDicEnumerator = DataManager.Instance.BackGroundDataDic.GetEnumerator();
            while (bgDataDicEnumerator.MoveNext())
            {
                tempBGIndexList.Add(bgDataDicEnumerator.Current.Key);
            }

            tempBGIndexList.Sort(delegate (int A, int B)
            {
                if (A > B)
                    return 1;
                else
                    return -1;
            });

            SkinIndexDic.Add(CommonData.SKIN_TYPE.BACKGROUND, tempBGIndexList);
        }

        Initialize();

        OnClickSlot(SkinList[0].mSkinData.id);
    }

    public void Initialize()
    {
        var skinIndexList = SkinIndexDic[SelectSkinType];

        for (int i = 0; i < SkinList.Count; i++)
        {
            DestroyImmediate(SkinList[i].gameObject);
        }
        SkinList.Clear();

        for (int i = 0; i < skinIndexList.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Prefab/UIShopSkinSlot"), SkinGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIShopSkinSlot>();
            slot.SetData(SelectSkinType, skinIndexList[i]);
            slot.SlotButton.onClick.AddListener(() => {OnClickSlot(slot.mSkinData.id);});
            SkinList.Add(slot);
        }
        ScrollRect.content.sizeDelta = new Vector2(SkinList.Count * 261, ScrollRect.content.sizeDelta.y);
    }

    public void OnClickSlot(int id)
    {
        for (int i = 0; i < SkinList.Count; i++)
        {
            if (SkinList[i].mSkinData.id == id)
                SkinList[i].SetSelect(true);
            else
                SkinList[i].SetSelect(false);
        }

        SelectSkinId = id;
        RefreshTopUI();
    }

    public void RefreshTopUI()
    {
        SkinData skinData = null;
        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinData = DataManager.Instance.CharDataDic[SelectSkinId];
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinData = DataManager.Instance.DoorDataDic[SelectSkinId];
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinData = DataManager.Instance.BackGroundDataDic[SelectSkinId];
                break;
            default:
                break;
        }
        if(skinData != null)
        {
            SelectSkinIcon.sprite = (Sprite)Resources.Load(skinData.GetIcon(), typeof(Sprite));
            SelectSkinDesc.text = skinData.desc;
        }
    }
    public void RefreshMidUI()
    {
        for (int i = 0; i < SkinList.Count; i++)
        {
            SkinList[i].RefreshUI();
        }
    }
    public void OnClickBuy()
    {
        UnityAction yesAction = () =>
        {
            PopupManager.Instance.DismissPopup();
            SkinData skinData = null;
            switch (SelectSkinType)
            {
                case CommonData.SKIN_TYPE.CHAR:
                    skinData = DataManager.Instance.CharDataDic[SelectSkinId];
                    break;
                case CommonData.SKIN_TYPE.DOOR:
                    skinData = DataManager.Instance.DoorDataDic[SelectSkinId];
                    break;
                case CommonData.SKIN_TYPE.BACKGROUND:
                    skinData = DataManager.Instance.BackGroundDataDic[SelectSkinId];
                    break;
                default:
                    break;
            }

            if (CommonFunc.UseCoin(skinData.cost))
            {
                switch (SelectSkinType)
                {
                    case CommonData.SKIN_TYPE.CHAR:
                        PlayerData.Instance.AddChar(SelectSkinId);
                        break;
                    case CommonData.SKIN_TYPE.DOOR:
                        PlayerData.Instance.AddDoor(SelectSkinId);
                        break;
                    case CommonData.SKIN_TYPE.BACKGROUND:
                        PlayerData.Instance.AddBG(SelectSkinId);
                        break;
                    default:
                        break;
                }
                
                RefreshMidUI();
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_SKIN_TITLE"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    }
    public void OnClickEquip()
    {
        if (IsHaveSkin() == false)
            return;

        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                PlayerData.Instance.SetUseCharId(SelectSkinId);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                PlayerData.Instance.SetUseDoorId(SelectSkinId);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                PlayerData.Instance.SetUseBGId(SelectSkinId);
                break;
            default:
                break;
        }
        RefreshMidUI();
    }

    private bool IsHaveSkin()
    {
        bool returnValue = false;
        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                returnValue = PlayerData.Instance.IsHasChar(SelectSkinId);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                returnValue = PlayerData.Instance.IsHasDoor(SelectSkinId);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                returnValue = PlayerData.Instance.IsHasBG(SelectSkinId);
                break;
            default:
                break;
        }

        if(returnValue == false)
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("NOT_BUY_SKIN")));
        }

        return returnValue;
    }
}
