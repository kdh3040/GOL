using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Vector3 OriginalScale;
    void Start()
    {
        OriginalScale = gameObject.transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = new Vector3(OriginalScale.x * 0.8f, OriginalScale.y * 0.8f, OriginalScale.z * 0.8f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = OriginalScale;
    }

}

