﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GManager : MonoBehaviour
{
    public static GManager _instance = null;
    public static GManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GManager>() as GManager;
            }
            return _instance;
        }
    }

    // 게임의 전체를 관리하는 매니저
    public PlayerData mPlayerData = new PlayerData();

    private bool bViewVideo = false;
    public WebViewObject webViewObject;

    void Start()
    {
        DontDestroyOnLoad(this);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Application.targetFrameRate = 60;

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, false);

        PlayerData.Instance.Initialize();
      //  PurchaseManager.Instance.InitializePurchasing();

        StartCoroutine(UpdateGame());
    }
    
    IEnumerator UpdateGame()
    {
        while (true)
        {
            var time = Time.deltaTime;
            PlayerData.Instance.UpdatePlayerData(time);
            yield return null;
        }
    }

    public void SetVideoStatus(bool ViewVideo)
    {
        bViewVideo = ViewVideo;
    }
    public bool GetVideoStatus()
    {
        return bViewVideo;
    }
}
