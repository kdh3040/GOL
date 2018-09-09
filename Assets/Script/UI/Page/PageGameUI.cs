using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PageGameUI : MonoBehaviour
{
    public UICountImgFont Score;
    public Text Info;
    public Button ItemButton;
    public Image ItemImg;
    public List<Image> ShieldIconList = new List<Image>();
    public Transform SkillProgressStartPos;
    public Button GamePauseButton;
    public UICountImgFont GameResumeCount;
    

    public GameObject CharMsgObj;
    public Text CharMsg;
    private float CharMsgViewInterval;

    private List<UISkillProgressBar> mSkillProgressBarList = new List<UISkillProgressBar>();

    public GameObject DoorButtons_1;
    public List<Button> DoorButtonsList_1 = new List<Button>();
    public GameObject DoorButtons_2;
    public List<Button> DoorButtonsList_2 = new List<Button>();


    void Awake()
    {
        if (GamePlayManager.Instance.GameOriginalMode)
            ItemButton.onClick.AddListener(OnClickItem);
        else
        {
            ItemImg.gameObject.SetActive(false);
            ItemButton.gameObject.SetActive(false);
        }
            
        GamePauseButton.onClick.AddListener(OnClickPause);

        if (GamePlayManager.Instance.GameOriginalMode)
        {
            DoorButtons_1.gameObject.SetActive(true);
            DoorButtons_2.gameObject.SetActive(false);
            for (int i = 0; i < DoorButtonsList_1.Count; i++)
            {
                int index = i;
                DoorButtonsList_1[i].onClick.AddListener(() => { OnClickDoorButton(index); });
            }
        }
        else if (GamePlayManager.Instance.GameOriginalMode == false)
        {
            DoorButtons_1.gameObject.SetActive(false);
            DoorButtons_2.gameObject.SetActive(true);
            for (int i = 0; i < DoorButtonsList_2.Count; i++)
            {
                int index = i;
                DoorButtonsList_2[i].onClick.AddListener(() => { OnClickDoorButton(index); });
            }
        }
    }

    public void ResetUI()
    {
        CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
        RefreshItemUI();
        RefreshShieldItemUI();
        Score.SetValue("0", UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshUI()
    {
        Score.SetValue(GamePlayManager.Instance.Score.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
    }

    public void RefreshItemUI()
    {
        if(GamePlayManager.Instance.GameOriginalMode)
        {
            int ItemId = GamePlayManager.Instance.UseItemId;
            if (ItemId != 0)
            {
                ItemImg.sprite = ItemManager.Instance.GetItemIcon(ItemId);
                ItemImg.color = new Color(1, 1, 1, 1);
            }
            else
            {
                ItemImg.color = new Color(1, 1, 1, 0);
                ItemImg.sprite = null;
            }
        }
    }

    public void RefreshShieldItemUI()
    {
        var skill = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
        for (int i = 0; i < ShieldIconList.Count; i++)
        {
            if (skill != null && skill.mCount >= i + 1)
            {
                ShieldIconList[i].gameObject.SetActive(true);
            }
            else
            {
                ShieldIconList[i].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshItemSkillUI()
    {
        for (int i = mSkillProgressBarList.Count - 1; i >= 0 ; --i)
        {
            if(mSkillProgressBarList[i].GetUISkillData() == null)
            {
                DestroyImmediate(mSkillProgressBarList[i].gameObject);
                mSkillProgressBarList.RemoveAt(i);
            }
            else
                mSkillProgressBarList[i].UpdateSkillProgress();
        }
    }

    public void OnClickItem()
    {
        GamePlayManager.Instance.UseGameNormalItem();
    }

    public void UseItemSkill(int itemId, GameSkill skill)
    {
        for (int i = 0; i < mSkillProgressBarList.Count; i++)
        {
            if (mSkillProgressBarList[i].mItemId == itemId)
                return;
        }

        var obj = Instantiate(Resources.Load("Prefab/UISkillProgressBar"), SkillProgressStartPos) as GameObject;
        var progressBar = obj.GetComponent<UISkillProgressBar>();
        progressBar.SetItemSkill(itemId, skill.mSkillType);
        progressBar.gameObject.transform.localPosition = new Vector3(0, mSkillProgressBarList.Count * 80);
        mSkillProgressBarList.Add(progressBar);
    }

    public void GameResume()
    {
        CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
        GameResumeCount.gameObject.SetActive(true);
        StartCoroutine(Co_GameResume());
    }

    public IEnumerator Co_GameResume()
    {
        GamePauseButton.gameObject.SetActive(false);
        int waitTime = 3;

        while(waitTime > 0)
        {
            GameResumeCount.SetValue(waitTime.ToString(), UICountImgFont.IMG_RANGE.CENTER, UICountImgFont.IMG_TYPE.YELLOW);
            yield return new WaitForSecondsRealtime(1f);
            waitTime--;
        }

        GamePauseButton.gameObject.SetActive(true);
        GameResumeCount.gameObject.SetActive(false);
        GamePlayManager.Instance.GameResume();
    }

    public void OnClickPause()
    {
        GamePlayManager.Instance.GamePause();
        PopupManager.Instance.ShowPopup(PopupManager.POPUP_TYPE.GAME_PAUSE);
    }

    public void GameOver()
    {
    }

    

    public void RefreshCharMsgUI(float time)
    {
        if (GamePlayManager.Instance.IsGamePause)
            return;

        CharMsgViewInterval -= time;
        if (CharMsgViewInterval < 0)
        {
            CharMsgViewInterval = ConfigData.Instance.CHAR_MSG_VIEW_INTERVAL;
            bool charMsgViewEnable = ConfigData.Instance.CHAR_MSG_VIEW_PERCENT > Random.Range(1, 100);
            if(charMsgViewEnable)
            {
                var randId = Random.Range(1, DataManager.Instance.CharMsgDataList.Count);
                ShowCharMsg(randId);
            }
        }
    }

    public void ShowCharMsg(int id)
    {
        CharMsgObj.SetActive(true);
        var data = DataManager.Instance.CharMsgDataList[id];
        CharMsg.text = LocalizeData.Instance.GetLocalizeString(data.Msg);
        StartCoroutine(Co_CharMsgDisable());
    }

    public IEnumerator Co_CharMsgDisable()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        CharMsgObj.gameObject.SetActive(false);
    }

    public void OnClickDoorButton(int index)
    {
        GamePlayManager.Instance.ClickDoorButton(index);
    }
#if UNITY_EDITOR
    void Update()
    {

        StringBuilder text = new StringBuilder();
        var skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SPEED_DOWN);
        if (skillData != null)
        {
            text.AppendFormat("노트속도 : {0:f2}", GamePlayManager.Instance.NoteSpeed * skillData.mPercent);
            text.AppendLine();
        }
        else
        {
            text.AppendFormat("노트속도 : {0:f2}", GamePlayManager.Instance.NoteSpeed);
            text.AppendLine();
        }

        // 스킬
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME);
        if(skillData != null)
        {
            text.AppendFormat("무적스킬 남은시간 {0:f2}", skillData.mTime);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.DAMAGE_SHIELD_COUNT);
        if (skillData != null)
        { 
            text.AppendFormat("쉴드스킬 남은갯수 {0}", skillData.mCount);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.SCORE_UP);
        if (skillData != null)
        { 
            text.AppendFormat("점수두배스킬 남은시간 {0:f2} {1:f2}배", skillData.mTime, skillData.mPercent);
            text.AppendLine();
        }

        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.RESURRECTION);
        if (skillData != null)
        {
            text.AppendFormat("부활스킬 발동중");
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_SCORE_BONUS);
        if (skillData != null)
        {
            text.AppendFormat("게임오버시 점수 상승 스킬 {0:f2}배", skillData.mPercent);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.ITEM_CREATE);
        if (skillData != null)
        {
            text.AppendFormat("아이템 노트 생성 스킬 {0:f2}퍼센트", skillData.mPercent * 100);
            text.AppendLine();
        }
        skillData = null;
        skillData = SkillManager.Instance.GetGameSkill(SkillManager.SKILL_TYPE.GAME_OVER_COIN_BONUS);
        if (skillData != null)
        {
            text.AppendFormat("게임오버시 코인 상승 스킬 {0:f2}배", skillData.mPercent);
            text.AppendLine();
        }

        Info.text = text.ToString();

    }
#endif
}
