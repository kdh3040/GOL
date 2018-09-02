using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameCenterManager : MonoBehaviour {


    public static GameCenterManager _instance = null;
    public static GameCenterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameCenterManager();
            }
            return _instance;
        }
    }

    GameCenterManager()
    {
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {

#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
         .EnableSavedGames()
         .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();

#elif UNITY_IOS
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true); 
#endif
    }

    public void SignIn()
    {
#if UNITY_ANDROID

 
        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("!!!!!! success");
                // to do ...
                // 구글 플레이 게임 서비스 로그인 성공 처리
            }
            else
            {
                Debug.Log("!!!!!! fail");
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

    public void ShowLeaderboardUI()
    {
        {
            // Sign In 이 되어있지 않은 상태라면
            // Sign In 후 리더보드 UI 표시 요청할 것
            if (Social.localUser.authenticated == false)
            {
                Social.localUser.Authenticate((bool success) =>
                {
                    if (success)
                    {
                        // Sign In 성공
                        // 바로 리더보드 UI 표시 요청
                        Social.ShowLeaderboardUI();
                        return;
                    }
                    else
                    {
                        // Sign In 실패 
                        // 그에 따른 처리
                        return;
                    }
                });
            }

#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
        }

    }
        

}
