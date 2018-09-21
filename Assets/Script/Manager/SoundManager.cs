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
    public AudioClip[] mFxSound = new AudioClip[15];
    private AudioSource mFxAudio;
 
    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        mFxAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayFXSound(CommonData.SOUND_TYPE type)
    {
        if (PlayerData.Instance.GetSoundSetting() == true)
        {
            mFxAudio.clip = mFxSound[(int)type];
            mFxAudio.Play();
        }
    }

    public void PlayFXSound(AudioClip clip)
    {
        if (PlayerData.Instance.GetSoundSetting() == true)
        {
            mFxAudio.clip = clip;
            mFxAudio.Play();
        }
    }

}
