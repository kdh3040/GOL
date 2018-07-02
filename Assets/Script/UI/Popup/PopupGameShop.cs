using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameShop : PopupUI
{
    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_SHOP;
    }

    public class PopupData : PopupUIData
    {
        public bool GameStartReady;
        public PopupData(bool ready)
        {
            GameStartReady = ready;
        }
    }

    public UITopBar TopBar;
    public List<Button> MyItemSlotLIst = new List<Button>();
    public List<Image> MyItemSlotBGLIst = new List<Image>();
    public List<Image> MyItemSlotIconLIst = new List<Image>();
    public Button MyShieldItemSlot;
    public Image MyShieldItemSlotBG;
    public Image MyShieldItemSlotIcon;
    public Transform ItemSlotListStartPos;
    public Text ItemDesc;
    public Button ItemBuy;
    public Button ItemEquip;
    public Button ItemUpgrade;
    public Button GameStart;

    private List<UIShopItem> ItemList = new List<UIShopItem>();
    private int SelectItemId = 0;

    void Awake()
    {
        for (int i = 0; i < MyItemSlotLIst.Count; ++i)
        {
            int temp = i;
            MyItemSlotLIst[i].onClick.AddListener(() => { OnClickMyItemSlot(temp); });
        }

        MyShieldItemSlot.onClick.AddListener(OnClickMyShieldItemSlot);
        ItemBuy.onClick.AddListener(OnClickItemBuy);
        ItemEquip.onClick.AddListener(OnClickItemEquip);
        ItemUpgrade.onClick.AddListener(OnClickItemUpgrade);
        GameStart.onClick.AddListener(OnClickGameStart);
    }

    public override void ShowPopup(PopupUIData data)
    {
        var popupData = data as PopupData;

        GameStart.gameObject.SetActive(popupData.GameStartReady);

        if(ItemList.Count <= 0)
        {
            var itemIndexList = DataManager.Instance.ItemDataIndexList;
            for (int i = 0; i < itemIndexList.Count; i++)
            {
                var obj = Instantiate(Resources.Load("Prefab/UIShopItem"), ItemSlotListStartPos) as GameObject;
                var slot = obj.GetComponent<UIShopItem>();
                slot.gameObject.transform.localPosition = new Vector3(i * 200, 0);
                slot.SetItem(itemIndexList[i]);
                slot.SlotButton.onClick.AddListener(() => { OnClickItemSlot(slot.mItemData.id);});
                ItemList.Add(slot);
            }
        }


        OnClickItemSlot(ItemList[0].mItemData.id);

        TopBar.Initialize(true);
        MyItemSlotUI();
    }

    public void MyItemSlotUI()
    {
        var normalItemArr = GManager.Instance.mPlayerData.mNormalitemArr;
        for (int i = 0; i < normalItemArr.Length; i++)
        {
            if(normalItemArr[i] == 0)
            {
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_empty", typeof(Sprite));
                MyItemSlotIconLIst[i].gameObject.SetActive(false);
            }
            else
            {
                var itemData = DataManager.Instance.ItemDataDic[normalItemArr[i]];
                MyItemSlotIconLIst[i].gameObject.SetActive(true);
                MyItemSlotIconLIst[i].sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
                MyItemSlotBGLIst[i].sprite = (Sprite)Resources.Load("item_slot", typeof(Sprite));
            }   
        }

        var shieldItmeId = GManager.Instance.mPlayerData.mShielditem;
        if(shieldItmeId == 0)
        {
            MyShieldItemSlotIcon.gameObject.SetActive(false);
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_empty", typeof(Sprite));
        }
        else
        {
            MyShieldItemSlotBG.sprite = (Sprite)Resources.Load("shield_slot", typeof(Sprite));
            var itemData = DataManager.Instance.ItemDataDic[shieldItmeId];
            MyShieldItemSlotIcon.gameObject.SetActive(true);
            MyShieldItemSlotIcon.sprite = (Sprite)Resources.Load(itemData.icon, typeof(Sprite));
        }
    }

    public void OnClickItemSlot(int itemId)
    {
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].mItemData.id == itemId)
                ItemList[i].SetSelect(true);
            else
                ItemList[i].SetSelect(false);
        }

        SelectItemId = itemId;
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        ItemDesc.text = SkillManager.Instance.GetSkillDesc(itemData.skill);
    }
    public void OnClickMyItemSlot(int index)
    {
        if(GManager.Instance.mPlayerData.mNormalitemArr[index] != 0)
        {
            GManager.Instance.mPlayerData.mNormalitemArr[index] = 0;
            MyItemSlotUI();
        }
    }
    public void OnClickMyShieldItemSlot()
    {
        GManager.Instance.mPlayerData.mShielditem = 0;
        MyItemSlotUI();
    }
    public void OnClickItemBuy()
    {
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        UnityAction yesAction = () =>
        {
            if (CommonFunc.IsEnoughCoin(itemData.cost, true))
            {
                GManager.Instance.mPlayerData.AddItem(SelectItemId);
            }
        };
        UnityAction noAction = () =>
        {
            PopupManager.Instance.DismissPopup();
        };
        var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("BUY_ITEM_TITLE"), yesAction, noAction);
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);

    }
    public void OnClickItemEquip()
    {
        bool equipEnable = false;
        var itemData = DataManager.Instance.ItemDataDic[SelectItemId];
        if(itemData.slot_type == CommonData.ITEM_SLOT_TYPE.NORMAL)
        {
            var normalItemArr = GManager.Instance.mPlayerData.mNormalitemArr;
            for (int i = 0; i < normalItemArr.Length; i++)
            {
                if (normalItemArr[i] == 0)
                {
                    equipEnable = true;
                    normalItemArr[i] = SelectItemId;
                }
            }
        }
        else
        {
            if(GManager.Instance.mPlayerData.mShielditem == 0)
            {
                equipEnable = true;
                GManager.Instance.mPlayerData.mShielditem = SelectItemId;
            }
        }
        if(equipEnable)
            MyItemSlotUI();
        else
        {
            var msgPopupData = new PopupMsg.PopupData(LocalizeData.Instance.GetLocalizeString("EQUIP_ITEM_FULL"));
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
        }
    }
    public void OnClickItemUpgrade()
    {
        var msgPopupData = new PopupMsg.PopupData("임시 강화 버튼 입니다.");
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, msgPopupData);
    }
    public void OnClickGameStart()
    {
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    //public Button RestartButton;
    //public Button ExitButton;

    //public Button MyDongButton;
    //public Button MyCoinButton;


    //public Transform[] ItemPos = new Transform[3];

    //private int nItemCount = DataManager.Instance.ItemDataIndexList.Count;

    //void Awake()
    //{
    //    RestartButton.onClick.AddListener(OnClickRestart);
    //    ExitButton.onClick.AddListener(OnClickExit);

    //    MyDongButton.onClick.AddListener(OnClickMyDong);
    //    MyCoinButton.onClick.AddListener(OnClickMyCoin);

    //    for (int i = 0; i < nItemCount; i++)
    //    {
    //        CreateItem(ItemPos[i], DataManager.Instance.ItemDataIndexList[i]);
    //    }

    //}


    //public void CreateItem(Transform ItemPos, int ItemType)
    //{
    //    var obj = Instantiate(Resources.Load("Prefab/Item"), ItemPos) as GameObject;
    //    var Item = obj.GetComponent<ShopItem>();

    //    Item.SetItemData(ItemPos, ItemType);

    //}


    //public void OnClickRestart()
    //{
    //    GamePlayManager.Instance.GameRestart();
    //    PopupManager.Instance.DismissPopup();
    //}

    //public void OnClickExit()
    //{
    //    GamePlayManager.Instance.GameExit();
    //    PopupManager.Instance.DismissPopup();
    //    SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    //}

    //public void OnClickCancel()
    //{
    //    PopupManager.Instance.DismissPopup();
    //    GamePlayManager.Instance.GameContinue();
    //}

    //public void OnClickMyDong()
    //{
    //    // 똥 사는 팝업
    //}

    //public void OnClickMyCoin()
    //{
    //    // 코인 사는 팝업
    //}
}