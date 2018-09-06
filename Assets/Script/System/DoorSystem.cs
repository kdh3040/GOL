﻿using System.Collections;
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
        SetAllDoorState(Door.DOOR_STATE.CLOSE);
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
        SetAllDoorState(Door.DOOR_STATE.CLOSE);
    }

    public void SetAllDoorState(Door.DOOR_STATE state)
    {
        for (int i = 0; i < DoorList.Count; i++)
        {
            DoorList[i].SetDoorState(state);
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
            for (int DoorNum = 0; DoorNum < 3; DoorNum++)
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.CLOSE);

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
}
