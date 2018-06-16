using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageLoadingUI : MonoBehaviour
{
    public Text LoadingText;

    void Start()
    {
        StartCoroutine(LoadingData());
    }

    IEnumerator LoadingData()
    {
        yield return DataManager.Instance.LoadingData(LoadingText);
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        SceneManager.LoadScene("PopupUIScene", LoadSceneMode.Additive);

        // TODO 환웅 : 메인씬으로 이동
        SceneManager.UnloadSceneAsync("LodingScene");
    }
}
