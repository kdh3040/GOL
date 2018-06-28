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

    public void SetValue(int count, IMG_RANGE range)
    {
        string countSrt = count.ToString();
        int countLength = countSrt.Length;
        Image prevImg = null;

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
            var imgSprite = (Sprite)Resources.Load(imgfileName, typeof(Sprite));
            RectTransform rt = countImg.GetComponent<RectTransform>();
            rt.sizeDelta = imgSprite.rect.size;
            countImg.sprite = imgSprite;
            if (prevImg == null)
                countImg.gameObject.transform.localPosition = Vector3.zero;
            else
            {
                if(range == IMG_RANGE.LEFT)
                {
                    countImg.gameObject.transform.localPosition = new Vector3(prevImg.gameObject.transform.localPosition.x + prevImg.sprite.rect.size.x / 2 + imgSprite.rect.size.x / 2, 0);
                }
                else if(range == IMG_RANGE.RIGHT)
                {
                    countImg.gameObject.transform.localPosition = new Vector3(prevImg.gameObject.transform.localPosition.x - prevImg.sprite.rect.size.x / 2 - imgSprite.rect.size.x / 2, 0);
                }
                else
                {
                    //var 
                }
                
            }

            prevImg = countImg;
        } 
    }

}
