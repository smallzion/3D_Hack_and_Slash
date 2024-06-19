using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Q,
    W,
    E,
    R,
    IceCyclone
}

[System.Serializable]
public class SkillStatus
{
    public string name;
    public string description;
    public SkillType skillType;

    public SkillType Type { get; set; }

    public SkillStatus(string name, string description, SkillType skillType)
    {
        this.name = name;
        this.description = description;
        this.Type = skillType;
    }
}
