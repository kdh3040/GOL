using System.Collections;
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

    public enum IMG_TYPE
    {
        YELLOW,
        GREEN,
    }

    public void SetValue(string count, IMG_RANGE range, IMG_TYPE imgType)
    {
        string countSrt = count.ToString();
        int countLength = countSrt.Length;
        Image prevImg = null;
        int allWidthSize = 0;

        for (int i = 0; i < ImgFontList.Count; i++)
        {
            ImgFontList[i].gameObject.SetActive(false);
        }

        string formString = "";
        switch (imgType)
        {
            case IMG_TYPE.YELLOW:
                formString = "Renewal/UI/icon_number_1_{0}";
                break;
            case IMG_TYPE.GREEN:
                formString = "Renewal/UI/icon_number_2_{0}";
                break;
            default:
                break;
        }

        for (int i = 0; i < countLength; i++)
        {
            char oneStr = countSrt[i];
            if (oneStr == '+')
                oneStr = 'p';

            string imgfileName = string.Format(formString, oneStr);

            if(ImgFontList.Count <= i)
            {
                var obj = new GameObject();
                obj.AddComponent<Image>();
                Image tempImage = obj.GetComponent<Image>();
                ImgFontList.Add(tempImage);
                obj.gameObject.transform.SetParent(gameObject.transform);
                obj.gameObject.transform.localScale = new Vector3(1, 1, 1);
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

            currImgLocalPosition = new Vector3(prevImgLocalPosition.x + previmgSize.x / 2 + currimgSize.x / 2, 0);
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
        else if (range == IMG_RANGE.RIGHT)
        {
            for (int i = 0; i < countLength; i++)
            {
                Image currImg = ImgFontList[i];
                Vector3 currImgLocalPosition = currImg.gameObject.transform.localPosition;

                currImgLocalPosition = new Vector3(currImgLocalPosition.x - allWidthSize + ImgFontList[0].sprite.rect.size.x / 2, 0);
                currImg.gameObject.transform.localPosition = currImgLocalPosition;
            }
        }
    }

}
