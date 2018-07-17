using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameBook : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_BOOK;
    }

    public enum TAB_TYPE
    {
        NONE,
        CHAR,
        ENDING,
        DOOR,
        BG
    }

    public UITopBar TopBar;
    public UITabButton CharTab;
    public UITabButton EndingTab;
    public UITabButton DoorTab;
    public UITabButton BGTab;
    public TAB_TYPE TabType = TAB_TYPE.NONE;

    public Image SkinIcon;
    public Text SkinDesc;

    public GridLayoutGroup SkinShopGrid;
    public ScrollRect ScrollRect;

    private CommonData.SKIN_TYPE mSkinType = CommonData.SKIN_TYPE.NONE;
    private int mSelectSkinId = 0;
    private List<UIBookSkinSlot> mSkinSlotList = new List<UIBookSkinSlot>();
    private Dictionary<CommonData.SKIN_TYPE, List<int>> mSkinIndexDic = new Dictionary<CommonData.SKIN_TYPE, List<int>>();

    void Awake()
    {
        CharTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.CHAR); });
        EndingTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.ENDING); });
        DoorTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.DOOR); });
        BGTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.BG); });
    }

    public override void ShowPopup(PopupUIData data)
    {
        if (mSkinIndexDic.Count <= 0)
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

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.CHAR, tempCharIndexList);

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

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.DOOR, tempDoorIndexList);

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

            mSkinIndexDic.Add(CommonData.SKIN_TYPE.BACKGROUND, tempBGIndexList);
        }

        TopBar.Initialize(true);
        OnClickTab(TAB_TYPE.CHAR);
    }

    public void OnClickTab(TAB_TYPE type)
    {
        if (TabType == type)
            return;

        TabType = type;
        mSkinType = ConvertBookTab(TabType);

        CharTab.SetSelect(TabType == TAB_TYPE.CHAR);
        EndingTab.SetSelect(TabType == TAB_TYPE.ENDING);
        DoorTab.SetSelect(TabType == TAB_TYPE.DOOR);
        BGTab.SetSelect(TabType == TAB_TYPE.BG);

        Initialize();
        OnClickSlot(mSkinSlotList[0].mSkinData.id);
    }

    public void Initialize()
    {
        var tempList = new List<int>();

        tempList = mSkinIndexDic[mSkinType];

        for (int i = 0; i < mSkinSlotList.Count; i++)
        {
            DestroyImmediate(mSkinSlotList[i].gameObject);
        }

        mSkinSlotList.Clear();

        for (int i = 0; i < tempList.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Prefab/UIBookSkinSlot"), SkinShopGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIBookSkinSlot>();
            mSkinSlotList.Add(slot);
            slot.SetData(mSkinType, tempList[i]);
            slot.SlotButton.onClick.AddListener(() => {
                if (slot.mSkinData != null)
                    OnClickSlot(slot.mSkinData.id);
            });
        }

        ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, (mSkinSlotList.Count / 4) * 530);
    }

    public CommonData.SKIN_TYPE ConvertBookTab(TAB_TYPE type)
    {
        switch (type)
        {
            case TAB_TYPE.CHAR:
                return CommonData.SKIN_TYPE.CHAR;
            case TAB_TYPE.ENDING:
                return CommonData.SKIN_TYPE.ENDING;
            case TAB_TYPE.DOOR:
                return CommonData.SKIN_TYPE.DOOR;
            case TAB_TYPE.BG:
                return CommonData.SKIN_TYPE.BACKGROUND;
            default:
                break;
        }

        return CommonData.SKIN_TYPE.NONE;
    }

    public void OnClickSlot(int id)
    {
        for (int i = 0; i < mSkinSlotList.Count; i++)
        {
            if (mSkinSlotList[i].mSkinData != null &&
                mSkinSlotList[i].mSkinData.id == id)
                mSkinSlotList[i].SetSelect(true);
            else
                mSkinSlotList[i].SetSelect(false);
        }

        mSelectSkinId = id;
        RefreshTopUI();
    }

    public void RefreshTopUI()
    {
        SkinData skinData = null;
        switch (mSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinData = DataManager.Instance.CharDataDic[mSelectSkinId];
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinData = DataManager.Instance.DoorDataDic[mSelectSkinId];
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinData = DataManager.Instance.BackGroundDataDic[mSelectSkinId];
                break;
            default:
                break;
        }
        if (skinData != null)
        {
            CommonFunc.SetImageFile(skinData.icon, ref SkinIcon);
            SkinDesc.text = skinData.desc;
        }
    }
}