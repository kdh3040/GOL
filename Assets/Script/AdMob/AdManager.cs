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
                _instance = new AdManager();
            }
            return _instance;
        }
    }

    public string android_interstitial_id;
    public string ios_interstitial_id;
    
    private static InterstitialAd interstitialAd;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RequestInterstitialAd()
    {


        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-4020702622451243/6018937389";
#elif UNITY_IOS
        adUnitId = "ca-app-pub-4020702622451243/5620865023";
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
        UnityEngine.Random.Range(0, CommonData.PROB_MAX_ADVIEW);

        if(UnityEngine.Random.Range(0, CommonData.PROB_MAX_ADVIEW) < CommonData.PROB_SELECT_ADVIEW)
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
}
