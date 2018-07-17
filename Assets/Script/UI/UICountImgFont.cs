﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICountImgFont : MonoBehaviour
{
    public Transform StartPos;

    private List<Image> ImgFontList = new List<Image>();

    public enum IMG_RANGE
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public void SetValue(int count, IMG_RANGE range)
    {
        string countSrt = count.ToString();
        int countLength = countSrt.Length;
        Image prevImg = null;
        int allWidthSize = 0;

        for (int i = 0; i < ImgFontList.Count; i++)
        {
            ImgFontList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < countLength; i++)
        {
            string imgfileName = string.Format("n{0}", countSrt[i]);

            if(ImgFontList.Count <= i)
            {
                var obj = new GameObject();
                obj.AddComponent<Image>();
                Image tempImage = obj.GetComponent<Image>();
                ImgFontList.Add(tempImage);
                obj.gameObject.transform.SetParent(gameObject.transform);
            }

            Image countImg = ImgFontList[i];
            countImg.gameObject.SetActive(true);
            CommonFunc.SetImageFile(imgfileName, ref countImg);
            allWidthSize += (int)countImg.sprite.rect.size.x;
        }

        for (int i = 0; i < countLength; i++)
        {
            Image currImg = ImgFontList[i];
            Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;
            Vector3 prevImgLocalPosition = prevImg == null ? Vector3.zero : prevImg.gameObject.transform.localPosition;
            Vector2 currimgSize = currImg.sprite.rect.size;
            Vector2 previmgSize = prevImg == null ? Vector2.zero : prevImg.sprite.rect.size;

            if (range == IMG_RANGE.LEFT)
            {
                currImgLocalPosition = new Vector3(prevImgLocalPosition.x + previmgSize.x / 2 + currimgSize.x / 2, 0);
            }
            else if (range == IMG_RANGE.RIGHT)
            {
                currImgLocalPosition = new Vector3(prevImgLocalPosition.x - previmgSize.x / 2 - currimgSize.x / 2, 0);
            }
            else
            {

                currImgLocalPosition = new Vector3(prevImgLocalPosition.x + previmgSize.x / 2 + currimgSize.x / 2, 0);
            }

            currImg.gameObject.transform.localPosition = currImgLocalPosition;

            prevImg = currImg;
        }

        if(range == IMG_RANGE.CENTER)
        {
            for (int i = 0; i < countLength; i++)
            {
                Image currImg = ImgFontList[i];
                Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;

                currImgLocalPosition = new Vector3(currImgLocalPosition.x - allWidthSize / 2 + ImgFontList[0].sprite.rect.size.x / 2, 0);
                currImg.gameObject.transform.localPosition = currImgLocalPosition;
            }
        }
    }

}