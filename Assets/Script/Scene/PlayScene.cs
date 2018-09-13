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
    public Transform NoteGroupEndViewPos;
    public PageGameUI UIPage;
    public PlayerChar PlayerCharObj;

    public SpriteRenderer Background_Back;
    public SpriteRenderer Background_Front;

    public GameObject NoteDeleteObj;
    public GameObject DDongViewObj;

    public GameObject InGameEffect_Slow;
    public GameObject InGameEffect_Double;
    public GameObject InGameEffect_Revive;

    public GameObject InGameEffect_Start;

    public AudioSource mAudio;
    public AudioClip[] mClip = new AudioClip[4];

    void Start()
    {
        var backgroundData = DataManager.Instance.BackGroundDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.BACKGROUND)];
        Background_Back.sprite = (Sprite)Resources.Load(backgroundData.img_back, typeof(Sprite));
        Background_Front.sprite = (Sprite)Resources.Load(backgroundData.img_front, typeof(Sprite));

        // 데이터 할당
        GamePlayManager.Instance.Initialize(this);
        // 게임시작
        GamePlayManager.Instance.GameStart();

        PlayBGM();
    }

    public void PlayBGM()
    {
        if (PlayerData.Instance.GetSoundSetting() == true)
        {

            SkinData mSkin = PlayerData.Instance.GetUseSkinData(CommonData.SKIN_TYPE.BACKGROUND);
            var backgoundData = mSkin as BackgroundData;

            mAudio.clip = mClip[backgoundData.id - 1];
            mAudio.Play();
        }
    }
}
