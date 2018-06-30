using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Button RestartButton;
    public Button ExitButton;

    public Button MyDongButton;
    public Button MyCoinButton;


    public Transform[] ItemPos = new Transform[3];

    private int nItemCount = DataManager.Instance.ItemDataIndexList.Count;

    void Awake()
    {
        RestartButton.onClick.AddListener(OnClickRestart);
        ExitButton.onClick.AddListener(OnClickExit);

        MyDongButton.onClick.AddListener(OnClickMyDong);
        MyCoinButton.onClick.AddListener(OnClickMyCoin);

        for (int i = 0; i < nItemCount; i++)
        {
            CreateItem(ItemPos[i], DataManager.Instance.ItemDataIndexList[i]);
        }
            
    }


    public void CreateItem(Transform ItemPos, int ItemType)
    {
        var obj = Instantiate(Resources.Load("Prefab/Item"), ItemPos) as GameObject;
        var Item = obj.GetComponent<ShopItem>();

        Item.SetItemData(ItemPos, ItemType);

    }


    public void OnClickRestart()
    {
        GamePlayManager.Instance.GameRestart();
        PopupManager.Instance.DismissPopup();
    }

    public void OnClickExit()
    {
        GamePlayManager.Instance.GameExit();
        PopupManager.Instance.DismissPopup();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void OnClickCancel()
    {
        PopupManager.Instance.DismissPopup();
        GamePlayManager.Instance.GameContinue();
    }

    public void OnClickMyDong()
    {
        // 똥 사는 팝업
    }

    public void OnClickMyCoin()
    {
        // 코인 사는 팝업
    }
}