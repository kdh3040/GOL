using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageLoadingUI : MonoBehaviour
{
    public Slider ProgressBar;
    public float WaitTime;
    void Start()
    {
        StartCoroutine(LoadingData());

    }

    IEnumerator LoadingData()
    {
        yield return DataManager.Instance.LoadingData(ProgressBar, WaitTime);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        SceneManager.LoadScene("PopupUIScene", LoadSceneMode.Additive);
    }
}
