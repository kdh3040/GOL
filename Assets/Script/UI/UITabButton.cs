using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour
{
    public Button TabButton;
    public Image SelectImg;
    public Image DeselectImg;
    public Text TabTitle;

    public void SetSelect(bool enable)
    {
        SelectImg.gameObject.SetActive(enable);
        DeselectImg.gameObject.SetActive(!enable);

        if(enable)
            TabTitle.color = new Color(0, 0, 0, 1);
        else
            TabTitle.color = new Color(1, 1, 1, 1);
    }

    public void SetTabTitle(string title)
    {
        TabTitle.text = LocalizeData.Instance.GetLocalizeString(title);
    }

}
