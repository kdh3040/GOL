using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    public List<Door> DoorList = new List<Door>();
    public List<NoteGroup> NoteGroupList = new List<NoteGroup>();
    public Transform NoteGroupEndPos;
    public Transform NoteGroupOpenPos;
    public PageGameUI UIPage;
    public PlayerChar PlayerCharObj;

    public SpriteRenderer Background;

    void Start()
    {
        var backgroundData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        Background.sprite = (Sprite)Resources.Load(backgroundData.img, typeof(Sprite));

        // 데이터 할당
        GamePlayManager.Instance.Initialize(this);
        // 게임시작
        GamePlayManager.Instance.GameStart();
    }
}
