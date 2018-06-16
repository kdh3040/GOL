using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageMainUI : MonoBehaviour {
    public Button Start;

    void Awake()
    {
        Start.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
