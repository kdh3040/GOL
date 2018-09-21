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
                _instance = FindObjectOfType<GameCenterManager>() as GameCenterManager;
            }
            return _instance;
        }
    }

    GameCenterManager()
    {
    }



    private int[] Achievement_score = { 1000000, 500000, 300000, 100000, 50000, 30000, 20000, 10000, 7500, 1000 };
    private string[] Achievement_score_list = {
        GPGSIds.achievement_1000000, GPGSIds.achievement_500000, GPGSIds.achievement_300000,
        GPGSIds.achievement_100000, GPGSIds.achievement_50000, GPGSIds.achievement_30000,
        GPGSIds.achievement_20000, GPGSIds.achievement_10000, GPGSIds.achievement_7500,
        GPGSIds.achievement_1000 };
    
    private string[] Achievement_door_list =
        { GPGSIds.achievement_door_1, GPGSIds.achievement_door_2,
        GPGSIds.achievement_door_3, GPGSIds.achievement_door_4, GPGSIds.achievement_door_5,
        GPGSIds.achievement_door_6, GPGSIds.achievement_door_7, GPGSIds.achievement_door_8,
        GPGSIds.achievement_door_9, GPGSIds.achievement_door_10, GPGSIds.achievement_door_11};

    private string[] Achievement_char_list = { GPGSIds.achievement_char_2, GPGSIds.achievement_char_3, GPGSIds.achievement_char_4 };
    private string[] Achievement_map_list = { GPGSIds.achievement_pyramid, GPGSIds.achievement_sea, GPGSIds.achievement_space };


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        Init();
        SignIn();
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

        Debug.Log("!!!!!! SignIn");
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

        if (Social.localUser.authenticated == false)
        {
            Debug.Log("!!!!!! SignIn ShowLeaderboardUI");
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 업적 UI 표시 요청
                    Debug.Log("!!!!!! SignIn Suc");
                    Social.ShowLeaderboardUI();
                    return;
                }
                else
                {
                    Debug.Log("!!!!!! SignIn Fail");
                    // Sign In 실패 처리
                    return;
                }
            });
        }

#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_score);
        
        Debug.Log("!!!!!! ShowLeaderboardUI 2");
#elif UNITY_IOS
        GameCenterPlatform.ShowLeaderboardUI("Leaderboard_ID", UnityEngine.SocialPlatforms.TimeScope.AllTime);
#endif
       
    }

    public void UnlockAchievement_score(int score)
    {

        for(int i=0; i< Achievement_score.Length; i++)
        {
            if ( score >= Achievement_score[i])
            {
#if UNITY_ANDROID
                PlayGamesPlatform.Instance.ReportProgress(Achievement_score_list[i], 100f, null);
#elif UNITY_IOS
            Social.ReportProgress(Achievement_score_list[i], 100f, null);
#endif

                break;
            }
        }
    }

    public void UnlockAchievement_Item(CommonData.SKIN_TYPE type, int Idx)
    {
        switch (type)
        {
            case CommonData.SKIN_TYPE.CHAR:
                UnlockAchievement_Char(Idx - 2);
                break;
            case CommonData.SKIN_TYPE.DOOR:
                UnlockAchievement_Door(Idx - 2);
                break;
            case CommonData.SKIN_TYPE.BACKGROUND:
                UnlockAchievement_BackGround(Idx - 2);
                break;
            default:
                break;

        }
    }

    private void UnlockAchievement_Door(int Idx)
    {

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportProgress(Achievement_door_list[Idx], 100f, null);
#elif UNITY_IOS
            Social.ReportProgress(Achievement_door_list[Idx], 100f, null);
#endif
    }


    private void UnlockAchievement_BackGround(int Idx)
    {

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportProgress(Achievement_map_list[Idx], 100f, null);
#elif UNITY_IOS
            Social.ReportProgress(Achievement_map_list[Idx], 100f, null);
#endif
    }

    private void UnlockAchievement_Char(int Idx)
    {

#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportProgress(Achievement_char_list[Idx], 100f, null);
#elif UNITY_IOS
            Social.ReportProgress(Achievement_char_list[Idx], 100f, null);
#endif
    }

    public void ShowAchievementUI()
    {
        // Sign In 이 되어있지 않은 상태라면
        // Sign In 후 업적 UI 표시 요청할 것
        if (Social.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // Sign In 성공
                    // 바로 업적 UI 표시 요청
                    Social.ShowAchievementsUI();
                    return;
                }
                else
                {
                    // Sign In 실패 처리
                    return;
                }
            });
        }

        Social.ShowAchievementsUI();
    }

    public void ReportScore(int score)
    {
#if UNITY_ANDROID

        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_score, (bool success) =>
        {
            if (success)
            {
                //Debug.Log("!!!!!! ReportScore 1");
                // Report 성공
                // 그에 따른 처리
            }
            else
            {
                //Debug.Log("!!!!!! ReportScore 2");
                // Report 실패
                // 그에 따른 처리
            }
        });

#elif UNITY_IOS
 
        Social.ReportScore(score, "Leaderboard_ID", (bool success) =>
            {
                if (success)
                {
                    // Report 성공
                    // 그에 따른 처리
                }
                else
                {
                    // Report 실패
                    // 그에 따른 처리
                }
            });
        
#endif
    }
}
