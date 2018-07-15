using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GManager : MonoBehaviour
{
    public static GManager _instance = null;
    public static GManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GManager>() as GManager;
            }
            return _instance;
        }
    }

    // 게임의 전체를 관리하는 매니저
    public PlayerData mPlayerData = new PlayerData();

    void Start()
    {
        DontDestroyOnLoad(this);
        PlayerData.Instance.Initialize();
        StartCoroutine(UpdateGame());
    }

    IEnumerator UpdateGame()
    {
        while (true)
        {
            var time = Time.deltaTime;
            PlayerData.Instance.UpdatePlayerData(time);
            yield return null;
        }
    }
}
