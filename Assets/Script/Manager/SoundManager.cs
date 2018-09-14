using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>() as SoundManager;
            }
            return _instance;
        }

    }

    private int soundFX;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        soundFX = AudioCenter.loadSound("Sound/click");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayFXSound()
    {
        if (PlayerData.Instance.GetSoundSetting() == true)
        {

            AudioCenter.playSound(soundFX);
            //mFX.Play();
        }
    }
}
