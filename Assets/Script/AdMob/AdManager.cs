using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour {

    public static AdManager _instance = null;
    public static AdManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdManager>() as AdManager;
            }
            return _instance;
        }
    }

    public string android_interstitial_id;
    public string ios_interstitial_id;
    
    private static InterstitialAd interstitialAd;
    private static RewardBasedVideoAd rewardBasedVideo;


    private static int PROB_SELECT_ADVIEW = 2;
    private static int PROB_MAX_ADVIEW = 10;

    private static string adAppID_Android = "ca-app-pub-4020702622451243~9202373650";
    private static string adAppID_Ios = "ca-app-pub-4020702622451243~6331311469";

    private static string adInterstitial_Android = "ca-app-pub-4020702622451243/6018937389";
    private static string adInterstitial_Ios = "ca-app-pub-4020702622451243/5620865023";

    private static string adVideo_Android = "ca-app-pub-4020702622451243/2922549491";
    private static string adVideo_Ios = "ca-app-pub-4020702622451243/1681620010";

    private static string adGameVideo_Android = "ca-app-pub-4020702622451243/7439989367";
    private static string adGameVideo_Ios = "ca-app-pub-4020702622451243/2207843982";
    private static RewardBasedVideoAd gameBasedVideo;


    private static string adDDongVideo_Android = "ca-app-pub-4020702622451243/9016794912";
    private static string adDDongVideo_Ios = "ca-app-pub-4020702622451243/4858058054";
    private static RewardBasedVideoAd DDongBasedVideo;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        RequestRewardBasedVideo();
        RequestInterstitialAd();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RequestInterstitialAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = adInterstitial_Android;
#elif UNITY_IOS
        adUnitId = adInterstitial_Ios;
#endif

        interstitialAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        
        interstitialAd.LoadAd(request);

        interstitialAd.OnAdClosed += HandleOnInterstitialAdClosed;
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        interstitialAd.Destroy();

        RequestInterstitialAd();
    }


    public void ShowInterstitialAd()
    {
        
        UnityEngine.Random.Range(0, PROB_MAX_ADVIEW);

        if(UnityEngine.Random.Range(0, PROB_MAX_ADVIEW) < PROB_SELECT_ADVIEW)
        {
            if (!interstitialAd.IsLoaded())
            {
                RequestInterstitialAd();
                return;
            }
            else
            {
                interstitialAd.Show();
            }
        }       
    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = adVideo_Android;
        MobileAds.Initialize(adAppID_Android);
#elif UNITY_IPHONE
        string adUnitId = adVideo_Ios;
        MobileAds.Initialize(adAppID_Ios);
#else
        string adUnitId = "unexpected_platform";
#endif
    
        rewardBasedVideo = RewardBasedVideoAd.Instance;

        AdRequest request = new AdRequest.Builder().Build();
        rewardBasedVideo.LoadAd(request, adUnitId);


#if UNITY_EDITOR
        string adGameUnitId = "unused";
#elif UNITY_ANDROID
        string adGameUnitId = adGameVideo_Android;
        MobileAds.Initialize(adGameVideo_Android);
#elif UNITY_IPHONE
        string adGameUnitId = adGameVideo_Ios;
        MobileAds.Initialize(adGameVideo_Ios);
#else
        string adGameUnitId = "unexpected_platform";
#endif


        gameBasedVideo = RewardBasedVideoAd.Instance;

        AdRequest requestGame = new AdRequest.Builder().Build();
        gameBasedVideo.LoadAd(requestGame, adGameUnitId);


#if UNITY_EDITOR
        string adDDongUnitId = "unused";
#elif UNITY_ANDROID
        string adDDongUnitId = adDDongVideo_Android;
        MobileAds.Initialize(adDDongVideo_Android);
#elif UNITY_IPHONE
        string adDDongUnitId = adDDongVideo_Ios;
        MobileAds.Initialize(adDDongVideo_Ios);
#else
        string adGameUnitId = "unexpected_platform";
#endif


        DDongBasedVideo = RewardBasedVideoAd.Instance;

        AdRequest requestDDong = new AdRequest.Builder().Build();
        DDongBasedVideo.LoadAd(requestDDong, adDDongUnitId);

    }

    public void ShowRewardVideo()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            //Subscribe to Ad event
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            rewardBasedVideo.Show();
            GamePlayManager.Instance.ContinueCount--;
        }
        else
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData("동영상 준비중입니다"));
        }
    }
    private void HandleRewardBasedVideoRewarded(object sender, Reward e)
    {
        Debug.Log("!!!!!! NotHandleRewardBasedVideoRewarded");
        StartCoroutine(GameRevival());
    }
    IEnumerator GameRevival()
    {
        yield return new WaitForSecondsRealtime(1f);
        GamePlayManager.Instance.GameRevival(false);
        PopupManager.Instance.DismissPopup();
    }

    public void ShowGameOverVideo()
    {
        UnityEngine.Random.Range(0, PROB_MAX_ADVIEW);

        if (UnityEngine.Random.Range(0, PROB_MAX_ADVIEW) < PROB_SELECT_ADVIEW)
        {
            if (!gameBasedVideo.IsLoaded())
            {
                RequestRewardBasedVideo();
            }
            else
            {
                gameBasedVideo.Show();
            }
        }        
    }

    public void ShowFreeDDongVideo()
    {
        if (!DDongBasedVideo.IsLoaded())
        {
            PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.MSG_POPUP, new PopupMsg.PopupData("동영상 준비중입니다"));
        }
        else
        {
            DDongBasedVideo.OnAdRewarded += HandleDDongBasedVideoRewarded;
            DDongBasedVideo.Show();
        }
    }

    private void HandleDDongBasedVideoRewarded(object sender, Reward e)
    {
        Debug.Log("!!!!!! NotHandleRewardBasedVideoRewarded");
        PlayerData.Instance.PlusDDong(1);
    }
}
