using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour
{
    public Button TabButton;
    public Image SelectImg;

    public void SetSelect(bool enable)
    {
        SelectImg.gameObject.SetActive(enable);
    }

}
