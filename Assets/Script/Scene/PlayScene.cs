using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScene : MonoBehaviour
{
    public List<Transform> mDoorPosList = new List<Transform>();
    public Transform NoteStartPos_1;
    public Transform NoteEndPos_1;
    public Transform NoteStartPos_2;
    public Transform NoteEndPos_2;
    public Transform NoteStartPos_3;
    public Transform NoteEndPos_3;
    public PageGameUI UIPage;
    public Transform NotesParentPos;

    void Start()
    {
        // 데이터 할당
        GamePlayManager.Instance.Initialize(this);
        // 게임시작
        GamePlayManager.Instance.GameStart();
    }
}
