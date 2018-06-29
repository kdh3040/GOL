using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image ItemImage;
    private ItemData data = null;

    public void SetItemData(Transform pos, int id)
    {
        data = DataManager.Instance.ItemDataDic[id];

        ItemImage.gameObject.transform.position = pos.position;
        ItemImage.sprite = (Sprite)Resources.Load(data.icon, typeof(Sprite));
    }
}
