using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageMainUI : MonoBehaviour {
    public Button Start;
    public Button Shop;

    void Awake()
    {
        Start.onClick.AddListener(OnClick);
        Shop.onClick.AddListener(OnClickShopBtn);
    }

    public void OnClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void OnClickShopBtn()
    {
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_SHOP);
    }

}
