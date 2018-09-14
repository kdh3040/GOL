using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIToastMsg : MonoBehaviour {

    public Text Msg;
    public CanvasGroup Group;
    public bool Empty = true;

    public void SetMsg(string msg)
    {
        Empty = false;
        Msg.text = msg;

        Group.alpha = Mathf.Lerp(1f, 1f, 1f);
        StartCoroutine(Co_ToastMsg());
    }

    IEnumerator Co_ToastMsg()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        time = 0f;
        var startPos = gameObject.transform.localPosition;
        while (time < 0.4f)
        {
            time += Time.deltaTime;
            gameObject.transform.localPosition = new Vector3(0, startPos.y + Mathf.Lerp(0f, 500f, time / 0.4f));
            Group.alpha = Mathf.Lerp(1f, 0.5f, time / 0.4f);
            yield return null;
        }

        gameObject.SetActive(false);
        Empty = true;

        //DestroyImmediate(this.gameObject);
    }
}
