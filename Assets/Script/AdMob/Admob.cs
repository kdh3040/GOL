using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Admob : MonoBehaviour {


    private BannerView AdBanner;

    // Use this for initialization
    void Start () {
        string adUnitID = "ca-app-pub-4020702622451243/9940740253";
        AdBanner = new BannerView(adUnitID, AdSize.SmartBanner, AdPosition.Top);
        AdRequest adRequest = new AdRequest.Builder().Build();
        AdBanner.LoadAd(adRequest);
        ShowAd();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowAd()
    {
        AdBanner.Show();
    }
}
