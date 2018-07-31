﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    public List<Door> DoorList = new List<Door>();
    public List<NoteGroup> NoteGroupList = new List<NoteGroup>();
    public Transform NoteGroupEndPos;
    public PageGameUI UIPage;
    public PlayerChar PlayerCharObj;

    public SpriteRenderer Background_Front;
    public SpriteRenderer Background_Back;

    void Start()
    {
        var backgroundData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.UseBGId];
        Background_Front.sprite = (Sprite)Resources.Load(backgroundData.img_front, typeof(Sprite));
        Background_Back.sprite = (Sprite)Resources.Load(backgroundData.img_back, typeof(Sprite));

        // 데이터 할당
        GamePlayManager.Instance.Initialize(this);
        // 게임시작
        GamePlayManager.Instance.GameStart();
    }
}
