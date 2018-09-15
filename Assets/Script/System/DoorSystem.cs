using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem
{
    public List<Door> DoorList = new List<Door>();

    public void Initialize(PlayScene scene)
    {
        DoorList = scene.DoorList;

        for (int i = 0; i < DoorList.Count; i++)
        {
            var type = (CommonData.NOTE_LINE)i;
            DoorList[i].SetData(type);
        }
    }


    public void ResetSystem()
    {
        SetAllDoorState(Door.DOOR_STATE.CLOSE, false);
        SetDoorEffect("IDLE");
    }

    public void GameStart()
    {
        ResetSystem();
    }

    public void GameRestart()
    {
        ResetSystem();
    }

    public void GameExit()
    {
        ResetSystem();
    }

    public void GameRevival()
    {
        SetAllDoorState(Door.DOOR_STATE.CLOSE, false);
    }

    public void SetAllDoorState(Door.DOOR_STATE state, bool closeEffect = true)
    {
        for (int i = 0; i < DoorList.Count; i++)
        {
            DoorList[i].SetDoorState(state, closeEffect);
        }
    }

    public void SetDoorState(CommonData.NOTE_LINE line, Door.DOOR_STATE state, bool closeEffect = true)
    {
        var DoorNum = (int)line;
        DoorList[DoorNum].SetDoorState(state, closeEffect);
    }

    public void PlaySound(CommonData.NOTE_LINE line)
    {
        var DoorNum = (int)line;
        DoorList[DoorNum].PlaySound();
    }

    public void StartSkillEffect(GameSkill skill)
    {
        if (skill.mSkillType == SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME)
        {
            SetAllDoorState(Door.DOOR_STATE.CLOSE, false);
            SetDoorEffect("INVINCIBILITY");
        }
    }

    public void EndSkillEffect(GameSkill skill)
    {
        if (skill.mSkillType == SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME)
            SetDoorEffect("IDLE");
    }

    public void ShowSkillEffect_Shield(CommonData.NOTE_LINE line)
    {
        SetDoorEffect(line, "SHIELD");
    }

    public void SetDoorEffect(CommonData.NOTE_LINE line, string trigger)
    {
        DoorList[(int)line].SetEffect(trigger); 
    }

    public void SetDoorEffect(string trigger)
    {
        for (int i = 0; i < DoorList.Count; i++)
        {
            var type = (CommonData.NOTE_LINE)i;
            SetDoorEffect(type, trigger);
        }
    }

    public void StopDoorEffect()
    {
        for (int i = 0; i < DoorList.Count; i++)
        {
            DoorList[i].StopEffectSound();
        }
    }
}
