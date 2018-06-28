using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image ItemImage;
    private ItemData data = null;
    protected Transform Pos;

    public void SetItemData(CommonData.NOTE_LINE type, Transform pos, int id)
    {
        data = DataManager.Instance.ItemDataDic[id];
        Pos = pos;

        ItemImage.gameObject.transform.position = Pos.position;
        ItemImage.sprite = (Sprite)Resources.Load(data.icon, typeof(Sprite));
    }
}
