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

    public abstract string GetIcon();
    
}
