using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour {
    public abstract PopupManager.POPUP_TYPE GetPopupType();
    public virtual void ShowPopup(PopupUIData data) { }
    public virtual void DismissPopup() { }
    public virtual void RefreshPopup() { }
    public Image BackGround;

    void Start()
    {
        PopupManager.Instance.AddPopup(this);
    }

    public void SetBackGroundImg()
    {
        if (BackGround == null)
            return;

        SkinData mSkin = PlayerData.Instance.GetUseSkinData(CommonData.SKIN_TYPE.BACKGROUND);
        var backgoundData = mSkin as BackgroundData;
        BackGround.sprite = (Sprite)Resources.Load(backgoundData.img_main, typeof(Sprite));
    }
}
