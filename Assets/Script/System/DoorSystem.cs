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

    public void SetDoorState(CommonData.NOTE_LINE line, int State)
    {
        var DoorNum = (int)line;
        switch(State)
        {
            case 0:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.OPEN);
                break;
            case 1:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.HALF_OPEN);
                break;
            case 2:
                DoorList[DoorNum].SetDoorState(Door.DOOR_STATE.CLOSE);
                break;
        }
    }

    public void PlaySound(CommonData.NOTE_LINE line)
    {
        var DoorNum = (int)line;
        DoorList[DoorNum].PlaySound();
    }

    public void StartSkillEffect(GameSkill skill)
    {
        if (skill.mSkillType == SkillManager.SKILL_TYPE.DAMAGE_SHIELD_TIME)
            SetDoorEffect("INVINCIBILITY");
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
