using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageLoadingUI : MonoBehaviour
{
    public Slider ProgressBar;

    void Start()
    {
        StartCoroutine(LoadingData());
    }

    IEnumerator LoadingData()
    {
        yield return DataManager.Instance.LoadingData(ProgressBar);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        SceneManager.LoadScene("PopupUIScene", LoadSceneMode.Additive);
    }
}
