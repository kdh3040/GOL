using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameEndingBook : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_BOOK;
    }

    public UITopBar TopBar;
    public Text Title;
    public GridLayoutGroup EndingSlotGrid;
    public ScrollRect ScrollRect;

    private List<KeyValuePair<int, int>> BackgroundEndingGroupList = new List<KeyValuePair<int, int>>();
    private List<UIEndingBookSlot> EndingBookSlotList = new List<UIEndingBookSlot>();

    public override void ShowPopup(PopupUIData data)
    {
        Title.text = LocalizeData.Instance.GetLocalizeString("POPUP_ENDING_BOOK_TITLE");
        TopBar.Initialize(true);

        if(BackgroundEndingGroupList.Count <= 0)
        {
            var dic = DataManager.Instance.BackGroundDataDic;
            var dicEnumerator = dic.GetEnumerator();
            while(dicEnumerator.MoveNext())
            {
                for (int i = 0; i < dicEnumerator.Current.Value.endingGroupList.Count; i++)
                {
                    int endingGroupId = dicEnumerator.Current.Value.endingGroupList[i];
                    BackgroundEndingGroupList.Add(new KeyValuePair<int, int>(dicEnumerator.Current.Key, endingGroupId));
                }
            }

            BackgroundEndingGroupList.Sort(delegate (KeyValuePair<int, int> A, KeyValuePair<int, int> B)
            {
                if (A.Key > B.Key)
                    return 1;
                else
                    return -1;
            });
        }

        SetEndingSlot();
    }

    private void SetEndingSlot()
    {
        for (int i = 0; i < EndingBookSlotList.Count; i++)
        {
            DestroyImmediate(EndingBookSlotList[i].gameObject);
        }
        EndingBookSlotList.Clear();

        for (int i = 0; i < BackgroundEndingGroupList.Count; i++)
        {
            var obj = Instantiate(Resources.Load("Prefab/UIEndingBookSlot"), EndingSlotGrid.gameObject.transform) as GameObject;
            var slot = obj.GetComponent<UIEndingBookSlot>();
            EndingBookSlotList.Add(slot);
            slot.SetData(BackgroundEndingGroupList[i].Key, BackgroundEndingGroupList[i].Value);
        }

        ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, EndingBookSlotList.Count * 366);
    }


    //public enum TAB_TYPE
    //{
    //    NONE,
    //    CHAR,
    //    ENDING,
    //    DOOR,
    //    BG
    //}

    //public UITopBar TopBar;
    //public UITabButton CharTab;
    //public UITabButton EndingTab;
    //public UITabButton DoorTab;
    //public UITabButton BGTab;
    //public TAB_TYPE TabType = TAB_TYPE.NONE;

    //public Image SkinIcon;
    //public Text SkinDesc;

    //public GridLayoutGroup SkinShopGrid;
    //public ScrollRect ScrollRect;

    //private CommonData.SKIN_TYPE mSkinType = CommonData.SKIN_TYPE.NONE;
    //private int mSelectSkinId = 0;
    //private List<UIBookSkinSlot> mSkinSlotList = new List<UIBookSkinSlot>();
    //private Dictionary<CommonData.SKIN_TYPE, List<int>> mSkinIndexDic = new Dictionary<CommonData.SKIN_TYPE, List<int>>();

    //void Awake()
    //{
    //    CharTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.CHAR); });
    //    EndingTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.ENDING); });
    //    DoorTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.DOOR); });
    //    BGTab.TabButton.onClick.AddListener(() => { OnClickTab(TAB_TYPE.BG); });
    //}

    //public override void ShowPopup(PopupUIData data)
    //{
    //    if (mSkinIndexDic.Count <= 0)
    //    {
    //        AddSkinIndexDic(CommonData.SKIN_TYPE.CHAR);
    //        AddSkinIndexDic(CommonData.SKIN_TYPE.ENDING);
    //        AddSkinIndexDic(CommonData.SKIN_TYPE.DOOR);
    //        AddSkinIndexDic(CommonData.SKIN_TYPE.BACKGROUND);

    //        CharTab.SetTabTitle("POPUP_BOOK_TAB_CHAR");
    //        EndingTab.SetTabTitle("POPUP_BOOK_TAB_ENDING");
    //        DoorTab.SetTabTitle("POPUP_BOOK_TAB_DOOR");
    //        BGTab.SetTabTitle("POPUP_BOOK_TAB_BACKGROUND");
    //    }

    //    TabType = TAB_TYPE.NONE;
    //    TopBar.Initialize(true);
    //    OnClickTab(TAB_TYPE.CHAR);
    //}

    //private void AddSkinIndexDic(CommonData.SKIN_TYPE type)
    //{
    //    var tempIndexList = new List<int>();

    //    switch (type)
    //    {
    //        case CommonData.SKIN_TYPE.CHAR:
    //            {
    //                var enumerator = DataManager.Instance.CharDataDic.GetEnumerator();
    //                while (enumerator.MoveNext())
    //                {
    //                    tempIndexList.Add(enumerator.Current.Key);
    //                }
    //            }
    //            break;
    //        case CommonData.SKIN_TYPE.DOOR:
    //            {
    //                var enumerator = DataManager.Instance.DoorDataDic.GetEnumerator();
    //                while (enumerator.MoveNext())
    //                {
    //                    tempIndexList.Add(enumerator.Current.Key);
    //                }
    //            }
    //            break;
    //        case CommonData.SKIN_TYPE.ENDING:
    //            {
    //                var enumerator = DataManager.Instance.EndingDataList.GetEnumerator();
    //                while (enumerator.MoveNext())
    //                {
    //                    tempIndexList.Add(enumerator.Current.Key);
    //                }
    //            }
    //            break;
    //        case CommonData.SKIN_TYPE.BACKGROUND:
    //            {
    //                var enumerator = DataManager.Instance.BackGroundDataDic.GetEnumerator();
    //                while (enumerator.MoveNext())
    //                {
    //                    tempIndexList.Add(enumerator.Current.Key);
    //                }
    //            }
    //            break;
    //        default:
    //            break;
    //    }

    //    tempIndexList.Sort(delegate (int A, int B)
    //    {
    //        if (A > B)
    //            return 1;
    //        else
    //            return -1;
    //    });

    //    mSkinIndexDic.Add(type, tempIndexList);
    //}

    //public void OnClickTab(TAB_TYPE type)
    //{
    //    if (TabType == type)
    //        return;

    //    TabType = type;
    //    mSkinType = ConvertBookTab(TabType);

    //    CharTab.SetSelect(TabType == TAB_TYPE.CHAR);
    //    EndingTab.SetSelect(TabType == TAB_TYPE.ENDING);
    //    DoorTab.SetSelect(TabType == TAB_TYPE.DOOR);
    //    BGTab.SetSelect(TabType == TAB_TYPE.BG);

    //    Initialize();
    //    OnClickSlot(mSkinSlotList[0].mSkinData.id);
    //}

    //public void Initialize()
    //{
    //    var tempList = new List<int>();

    //    tempList = mSkinIndexDic[mSkinType];

    //    for (int i = 0; i < mSkinSlotList.Count; i++)
    //    {
    //        DestroyImmediate(mSkinSlotList[i].gameObject);
    //    }

    //    mSkinSlotList.Clear();

    //    for (int i = 0; i < tempList.Count; i++)
    //    {
    //        var obj = Instantiate(Resources.Load("Prefab/UIBookSkinSlot"), SkinShopGrid.gameObject.transform) as GameObject;
    //        var slot = obj.GetComponent<UIBookSkinSlot>();
    //        mSkinSlotList.Add(slot);
    //        slot.SetData(mSkinType, tempList[i]);
    //        slot.SlotButton.onClick.AddListener(() => {
    //            if (slot.mSkinData != null)
    //                OnClickSlot(slot.mSkinData.id);
    //        });
    //    }

    //    int rowCount = 0;
    //    if ((mSkinSlotList.Count % 4) == 0)
    //        rowCount = (mSkinSlotList.Count / 4);
    //    else
    //        rowCount = (mSkinSlotList.Count / 4) + 1;
    //    ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, rowCount * 530);
    //}

    //public CommonData.SKIN_TYPE ConvertBookTab(TAB_TYPE type)
    //{
    //    switch (type)
    //    {
    //        case TAB_TYPE.CHAR:
    //            return CommonData.SKIN_TYPE.CHAR;
    //        case TAB_TYPE.ENDING:
    //            return CommonData.SKIN_TYPE.ENDING;
    //        case TAB_TYPE.DOOR:
    //            return CommonData.SKIN_TYPE.DOOR;
    //        case TAB_TYPE.BG:
    //            return CommonData.SKIN_TYPE.BACKGROUND;
    //        default:
    //            break;
    //    }

    //    return CommonData.SKIN_TYPE.NONE;
    //}

    //public void OnClickSlot(int id)
    //{
    //    for (int i = 0; i < mSkinSlotList.Count; i++)
    //    {
    //        if (mSkinSlotList[i].mSkinData != null &&
    //            mSkinSlotList[i].mSkinData.id == id)
    //            mSkinSlotList[i].SetSelect(true);
    //        else
    //            mSkinSlotList[i].SetSelect(false);
    //    }

    //    mSelectSkinId = id;
    //    RefreshTopUI();
    //}

    //public void RefreshTopUI()
    //{
    //    SkinData skinData = null;
    //    switch (mSkinType)
    //    {
    //        case CommonData.SKIN_TYPE.CHAR:
    //            skinData = DataManager.Instance.CharDataDic[mSelectSkinId];
    //            break;
    //        case CommonData.SKIN_TYPE.ENDING:
    //            skinData = DataManager.Instance.EndingDataList[mSelectSkinId];
    //            break;
    //        case CommonData.SKIN_TYPE.DOOR:
    //            skinData = DataManager.Instance.DoorDataDic[mSelectSkinId];
    //            break;
    //        case CommonData.SKIN_TYPE.BACKGROUND:
    //            skinData = DataManager.Instance.BackGroundDataDic[mSelectSkinId];
    //            break;
    //        default:
    //            break;
    //    }
    //    if (skinData != null)
    //    {
    //        CommonFunc.SetImageFile(skinData.GetIcon(), ref SkinIcon);
    //        SkinDesc.text = skinData.desc;
    //    }
    //}
}