using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupGameEnd : PopupUI {

    public override PopupManager.POPUP_TYPE GetPopupType()
    {
        return PopupManager.POPUP_TYPE.GAME_END;
    }
}
