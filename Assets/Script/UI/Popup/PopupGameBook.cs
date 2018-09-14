using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public enum BOOK_TYPE
    {
        NONE,
        ENDING,
        SKIN,
    }

    public UITopBar TopBar;
    public UITabButton EndingBookTab;
    public UITabButton SkinBookTab;

    public GameObject EndingBookObj;
    public GridLayoutGroup EndingSlotGrid;
    public ScrollRect ScrollRect;
    private List<KeyValuePair<int, int>> BackgroundEndingGroupList = new List<KeyValuePair<int, int>>();
    private List<UIEndingBookSlot> EndingBookSlotList = new List<UIEndingBookSlot>();

    public GameObject SkinBookObj;
    public Image SkinIcon;
    public Image SkinCharIcon;
    public Animator SkinCharIconAnim;
    public Text SkinDesc;
    public GridLayoutGroup SkinCharSlotGrid;
    public ScrollRect SkinCharScrollRect;
    public GridLayoutGroup SkinDoorSlotGrid;
    public ScrollRect SkinDoorScrollRect;
    public GridLayoutGroup SkinBgSlotGrid;
    public ScrollRect SkinBgScrollRect;

    private List<UIBookSkinSlot> SkinCharBookSlotList = new List<UIBookSkinSlot>();
    private List<UIBookSkinSlot> SkinDoorBookSlotList = new List<UIBookSkinSlot>();
    private List<UIBookSkinSlot> SkinBgBookSlotList = new List<UIBookSkinSlot>();
    private CommonData.SKIN_TYPE SelectSkinType = CommonData.SKIN_TYPE.NONE;
    private int SelectIndex = -1;

    private BOOK_TYPE SelectBookType = BOOK_TYPE.NONE;

    public void Awake()
    {
        EndingBookTab.TabButton.onClick.AddListener(() => { OnClickBook(BOOK_TYPE.ENDING); });
        SkinBookTab.TabButton.onClick.AddListener(() => { OnClickBook(BOOK_TYPE.SKIN); });
    }

    public override void ShowPopup(PopupUIData data)
    {
        TopBar.Initialize(true);
        SelectBookType = BOOK_TYPE.NONE;
        OnClickBook(BOOK_TYPE.ENDING);
    }

    public void OnClickBook(BOOK_TYPE type)
    {
       // SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);

        if (SelectBookType == type)
            return;

        SelectBookType = type;

        EndingBookObj.SetActive(false);
        SkinBookObj.SetActive(false);

        EndingBookTab.SetSelect(SelectBookType == BOOK_TYPE.ENDING);
        SkinBookTab.SetSelect(SelectBookType == BOOK_TYPE.SKIN);

        switch (SelectBookType)
        {
            case BOOK_TYPE.ENDING:
                ShowEndingBook();
                break;
            case BOOK_TYPE.SKIN:
                ShowSkinBook();
                break;
            default:
                break;
        }
    }

    public void ShowEndingBook()
    {
        EndingBookObj.SetActive(true);

        if (BackgroundEndingGroupList.Count <= 0)
        {
            var dic = DataManager.Instance.BackGroundDataDic;
            var dicEnumerator = dic.GetEnumerator();
            while (dicEnumerator.MoveNext())
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

        ScrollRect.content.sizeDelta = new Vector2(ScrollRect.content.sizeDelta.x, EndingBookSlotList.Count * 420);
    }

    public void ShowSkinBook()
    {
        SkinBookObj.SetActive(true);

        SetSkinSlotList(ref SkinCharBookSlotList, SkinCharSlotGrid, SkinCharScrollRect, CommonData.SKIN_TYPE.CHAR);
        SetSkinSlotList(ref SkinDoorBookSlotList, SkinDoorSlotGrid, SkinDoorScrollRect, CommonData.SKIN_TYPE.DOOR);
        SetSkinSlotList(ref SkinBgBookSlotList, SkinBgSlotGrid, SkinBgScrollRect, CommonData.SKIN_TYPE.BACKGROUND);

        OnClickSkinSlot(CommonData.SKIN_TYPE.CHAR, 0);
    }

    public void SetSkinSlotList(ref List<UIBookSkinSlot> list, GridLayoutGroup grid, ScrollRect scroll, CommonData.SKIN_TYPE skinType)
    {
        if(list.Count <= 0)
        {
            List<int> skinIdList = new List<int>();

            switch (skinType)
            {
                case CommonData.SKIN_TYPE.CHAR:
                    var charEnumerator = DataManager.Instance.CharDataDic.GetEnumerator();
                    while (charEnumerator.MoveNext())
                    {
                        skinIdList.Add(charEnumerator.Current.Key);
                    }
                    break;
                case CommonData.SKIN_TYPE.DOOR:
                    var doorEnumerator = DataManager.Instance.DoorDataDic.GetEnumerator();
                    while (doorEnumerator.MoveNext())
                    {
                        skinIdList.Add(doorEnumerator.Current.Key);
                    }
                    break;
                case CommonData.SKIN_TYPE.BACKGROUND:
                    var bgEnumerator = DataManager.Instance.BackGroundDataDic.GetEnumerator();
                    while (bgEnumerator.MoveNext())
                    {
                        skinIdList.Add(bgEnumerator.Current.Key);
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < skinIdList.Count; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIBookSkinSlot"), grid.gameObject.transform) as GameObject;
                var slot = obj.GetComponent<UIBookSkinSlot>();
                list.Add(slot);
                slot.SetSkinSlot(skinIdList[i], skinType);

                int index = i;
                CommonData.SKIN_TYPE type = skinType;
                slot.SlotButton.onClick.AddListener(() => { OnClickSkinSlot(type, index); });
            }

            scroll.content.sizeDelta = new Vector2(list.Count * 240, scroll.content.sizeDelta.y);
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].RefreshUI();
            }
        }
    }

    public void ResetSkinSlot()
    {
        for (int i = 0; i < SkinCharBookSlotList.Count; i++)
        {
            SkinCharBookSlotList[i].SetSelect(false);
        }
        for (int i = 0; i < SkinDoorBookSlotList.Count; i++)
        {
            SkinDoorBookSlotList[i].SetSelect(false);
        }
        for (int i = 0; i < SkinBgBookSlotList.Count; i++)
        {
            SkinBgBookSlotList[i].SetSelect(false);
        }
    }

    public void OnClickSkinSlot(CommonData.SKIN_TYPE type, int index)
    {
        //SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);

        if (SelectSkinType == type && SelectIndex == index)
            return;

        ResetSkinSlot();

        SelectSkinType = type;
        SelectIndex = index;

        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                SkinCharBookSlotList[SelectIndex].SetSelect(true);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                SkinDoorBookSlotList[SelectIndex].SetSelect(true);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                SkinBgBookSlotList[SelectIndex].SetSelect(true);
                break;
            default:
                break;
        }

        RefreshDesc();
    }

    public void RefreshDesc()
    {
        int skinId = 0;
        switch (SelectSkinType)
        {
            case CommonData.SKIN_TYPE.CHAR:
                skinId = SkinCharBookSlotList[SelectIndex].SkinId;
                break;
            case CommonData.SKIN_TYPE.DOOR:
                skinId = SkinDoorBookSlotList[SelectIndex].SkinId;
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                skinId = SkinBgBookSlotList[SelectIndex].SkinId;
                break;
            default:
                break;
        }
        SkinData data = DataManager.Instance.GetSkinData(SelectSkinType, skinId);
        var skinSkillName = data.GetSkillName();
        var skinSkillData = SkillManager.Instance.GetSkillData(skinSkillName);

        StringBuilder desc = new StringBuilder();
        desc.AppendFormat("{0}{1}", LocalizeData.Instance.GetLocalizeString("POPUP_GAME_SHOP_DESC_NAME"), data.GetLocalizeName());
        desc.AppendLine();
        desc.AppendLine();
        desc.AppendFormat(data.GetLocalizeDesc());
        desc.AppendLine();
        desc.AppendLine();
        if (skinSkillData.GetDesc() != "")
            desc.AppendFormat(skinSkillData.GetDesc());
        SkinDesc.text = desc.ToString();

        if (SelectSkinType != CommonData.SKIN_TYPE.CHAR)
        {
            SkinIcon.gameObject.SetActive(true);
            SkinCharIcon.gameObject.SetActive(false);
            CommonFunc.SetImageFile(data.GetIcon(), ref SkinIcon, false);
        }
        else
        {
            var charData = data as CharData;
            SkinIcon.gameObject.SetActive(false);
            SkinCharIcon.gameObject.SetActive(true);
            SkinCharIconAnim.Rebind();
            SkinCharIconAnim.SetTrigger(charData.shopani_trigger);
        }
    }

}