using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour {

    public static SettingManager _instance = null;
    public static SettingManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SettingManager();
            }
            return _instance;
        }
    }

    private bool mVibeStatus = true;
    private bool mSoundStatus = true;
    private bool mNotiStatus = true;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DoVibe()
    {
        if(mVibeStatus)
            Handheld.Vibrate();
    }

    public void SetVibeStatus(bool Status)
    {
        mVibeStatus = Status;
    }

    public bool GetVibeStatus()
    {
        return mVibeStatus;
    }
    
    public void SetSoundStatus(bool Status)
    {
        mSoundStatus = Status;
    }

    public bool GetSoundStatus()
    {
        return mSoundStatus;
    }

    public void SetNotiStatus(bool Status)
    {
        mNotiStatus = Status;
    }

    public bool GetNotiStatus()
    {
        return mNotiStatus;
    }
}
