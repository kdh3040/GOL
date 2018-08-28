using GooglePlayGames;
using GooglePlayGames.BasicApi;
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
        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, false);
        PlayerData.Instance.Initialize();

        if (!FirebaseManager.Instance.SingedInFirebase())
        {
            FirebaseManager.Instance.LogIn();
        }

        Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseManager.Instance.OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseManager.Instance.OnMessageReceived;



        
#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();
        SignIn();
        
#elif UNITY_IOS
 
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
#endif


        /*
         * PlayGamesClientConfiguration.Builder()
    // enables saving game progress.
    .EnableSavedGames()
    // registers a callback to handle game invitations received while the game is not running.
    .WithInvitationDelegate(<callback method>)
    // registers a callback for turn based match notifications received while the
    // game is not running.
    .WithMatchDelegate(<callback method>)
    // requests the email address of the player be available.
    // Will bring up a prompt for consent.
    .RequestEmail()
    // requests a server auth code be generated so it can be passed to an
    //  associated back end server application and exchanged for an OAuth token.
    .RequestServerAuthCode(false)
    // requests an ID token be generated.  This OAuth token can be used to
    //  identify the player to other services such as Firebase.
    .RequestIdToken()
    .Build();
         */

        // 안드로이드 빌더 초기화
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        //PlayGamesPlatform.InitializeInstance(config);

        //// 구글 플레이 로그를 확인할려면 활성화
        //PlayGamesPlatform.DebugLogEnabled = true;

        //// 구글 플레이 활성화
        //PlayGamesPlatform.Activate();


        // Callback 함수 정의
        //signInCallback = (bool success) =>
        //{
        //    if (success)
        //        stateText.text = "SignIn Success!";
        //    else
        //        stateText.text = "SignIn Fail!";
        //};


        /*
        ttt.text = "1";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .RequestServerAuthCode(false)
        .Build();
        ttt.text = "2";
        PlayGamesPlatform.InitializeInstance(config);
        ttt.text = "3";
        PlayGamesPlatform.DebugLogEnabled = true;
        ttt.text = "4";
        PlayGamesPlatform.Activate();
        ttt.text = "5";
        Social.localUser.Authenticate((result, errorMessage) => {
            if (result)
            {
                ttt.text = "성공";
                // 인증 성공
            }
            else
            {
                ttt.text = "실패" + errorMessage;
                // 인증 실패
            }
        });
        */

        StartCoroutine(UpdateGame());
    }


    public void SignIn()
    {
        

#if UNITY_ANDROID

        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
            }
            else
            {
                // to do ...
                // 구글 플레이 게임 서비스 로그인 실패 처리
            }
        });

#elif UNITY_IOS
 
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                // to do ...
                // 애플 게임 센터 로그인 성공 처리
            }
            else
            {
                // to do ...
                // 애플 게임 센터 로그인 실패 처리
            }
        });
 
#endif
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
