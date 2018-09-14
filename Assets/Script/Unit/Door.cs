using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DOOR_STATE
    {
        NONE,
        CLOSE,
        HALF_OPEN,
        OPEN,
    }

    public DoorData Data;
    public SpriteRenderer DoorSprite;
    public CommonData.NOTE_LINE NoteLineType;
    public DOOR_STATE DoorState = DOOR_STATE.NONE;
    public Animator DoorEffectAnim;
    public Animator DoorCloseEffect;
    public bool EffectPlay = false;

    public AudioClip[] mEffectAudio = new AudioClip[2];
    private AudioSource mAudio;

    public void SetData(CommonData.NOTE_LINE type)
    {
        Data = DataManager.Instance.DoorDataDic[PlayerData.Instance.GetUseSkin(CommonData.SKIN_TYPE.DOOR)];
        NoteLineType = type;
        EffectPlay = false;
        DoorState = DOOR_STATE.NONE;
        SetDoorState(DOOR_STATE.CLOSE, false);
    }

    public void SetDoorState(DOOR_STATE type, bool closeEffect = true)
    {
        if (EffectPlay)
        {
            if(closeEffect && type == DOOR_STATE.CLOSE)
            {
                SetCloseEffect();
                SetCloseMsgEffect();
            }
            return;
        }
            

        if (DoorState == type)
            return;

        DoorState = type;

        switch (DoorState)
        {
            case DOOR_STATE.CLOSE:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.close_img, typeof(Sprite));
                
                if (closeEffect)
                {
                    SetCloseEffect();
                    SetCloseMsgEffect();
                }
                    
                break;
            case DOOR_STATE.HALF_OPEN:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.halfopen_img, typeof(Sprite));
                break;
            case DOOR_STATE.OPEN:
                DoorSprite.sprite = (Sprite)Resources.Load(Data.open_img, typeof(Sprite));
                break;
            default:
                break;
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Note"))
        {
            SetDoorState(DOOR_STATE.HALF_OPEN);
        }
    }
    */

    public void PlaySound()
    {
        SoundManager.Instance.PlayFXSound(CommonData.SOUND_TYPE.DOOR);
    }

    public void PlayEffectSound(CommonData.DOOR_EFFECT_SOUND_TYPE type)
    {
        mAudio = GetComponent<AudioSource>();
        mAudio.clip = mEffectAudio[(int)type];
        mAudio.Play();
    }

    public void StopEffectSound()
    {
        mAudio = GetComponent<AudioSource>();
        mAudio.Stop();
    }

    public void SetCloseEffect()
    {
        DoorCloseEffect.Rebind();
        DoorCloseEffect.SetTrigger("Close");
    }

    public void SetCloseMsgEffect()
    {
        var obj = Instantiate(Resources.Load("Prefab/NoteDeleteMsg"), gameObject.transform) as GameObject;
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        sprite.sprite = (Sprite)Resources.Load(CommonData.NOTE_DELETE_MSG[Random.Range(0, CommonData.NOTE_DELETE_MSG.Length)], typeof(Sprite));
        obj.gameObject.transform.localPosition = new Vector3(0, -2, -4);
        StartCoroutine(Co_DeleteNoteMsg(obj));
    }

    IEnumerator Co_DeleteNoteMsg(GameObject obj)
    {
        float time = 0.5f;
        float saveTime = 0;
        while (saveTime < time)
        {
            saveTime += Time.deltaTime;
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y + 0.1f, obj.transform.localPosition.z);
            yield return null;
        }
        DestroyImmediate(obj);
    }

    public void SetEffect(string trigger)
    {
        DoorEffectAnim.Rebind();
        DoorEffectAnim.SetTrigger(trigger);

        switch (trigger)
        {        
            case "INVINCIBILITY":
                EffectPlay = true;
                PlayEffectSound(CommonData.DOOR_EFFECT_SOUND_TYPE.IRONDOOR);
                break;
            case "SHIELD":
                EffectPlay = false;
                PlayEffectSound(CommonData.DOOR_EFFECT_SOUND_TYPE.SHIELD);
                break;
            default:
                EffectPlay = false;
                StopEffectSound();
                break;
        } 
    }
}
