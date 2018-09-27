using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
 

public class UnityAdsHelper : MonoBehaviour {

    private const string android_game_id = "2783004";
    private const string ios_game_id = "2783003";

    private const string rewarded_video_id = "rewardedVideo";
    private const string free_video_id = "video";
    private const string revive_video_id = "rewardedVideo";

    public static UnityAdsHelper _instance = null;
    public static UnityAdsHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UnityAdsHelper>() as UnityAdsHelper;
            }
            return _instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Initialize();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
#elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
#endif
    }

    public void ShowGameOverAd()
    {
        if (Advertisement.IsReady(free_video_id))
        {
            Advertisement.Show(free_video_id);
        }
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewarded_video_id))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };

            Advertisement.Show(rewarded_video_id, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");

                    PlayerData.Instance.PlusDDong(1);
                    // to do ...
                    // 광고 시청이 완료되었을 때 처리

                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");

                    // to do ...
                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");

                    // to do ...
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }


    public void ShowReviveAd()
    {
        if (Advertisement.IsReady(rewarded_video_id))
        {
            var options = new ShowOptions { resultCallback = HandleReviveShowResult };
            Advertisement.Show(rewarded_video_id, options);
        }
    }

    IEnumerator GameRevival()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        GamePlayManager.Instance.GameRevival(false);
    }

    private void HandleReviveShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");

                    StartCoroutine(GameRevival());
                    // to do ...
                    // 광고 시청이 완료되었을 때 처리

                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");

                    // to do ...
                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");

                    // to do ...
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }

}
