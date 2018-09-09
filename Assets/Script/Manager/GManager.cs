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

    void Start()
    {
        DontDestroyOnLoad(this);

        Application.targetFrameRate = 60;

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, false);

   

        PlayerData.Instance.Initialize();
      //  PurchaseManager.Instance.InitializePurchasing();

        
        if (!FirebaseManager.Instance.SingedInFirebase())
        {
            FirebaseManager.Instance.LogIn();
        }

        Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseManager.Instance.OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseManager.Instance.OnMessageReceived;
        
     


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
}
