﻿using System.Collections;
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
        SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.BUTTON);
        this.gameObject.transform.localScale = new Vector3(OriginalScale.x * 0.9f, OriginalScale.y * 0.9f, OriginalScale.z * 0.9f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.gameObject.transform.localScale = OriginalScale;
    }

}

