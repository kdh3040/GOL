using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public abstract class SkinData
{
    public int id;
    public string name = "";
    public string desc = "";
    protected string icon = "";
    public int cost = 0;
    public string skill = "";

    public abstract string GetIcon();
    public abstract string GetSkinSlotTypeName();
    public string GetLocalizeName()
    {
        return LocalizeData.Instance.GetLocalizeString(name);
    }
    public string GetLocalizeDesc()
    {
        return LocalizeData.Instance.GetLocalizeString(desc);
    }
    public string GetSkillName()
    {
        if (skill == "")
            return "";

        string skillName = string.Format("{0}_LV{1}", skill, 1);
        return skillName;
    }
}
