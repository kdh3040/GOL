using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAds : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AdManager.Instance.RequestBannerAd();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
