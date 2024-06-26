using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType : int
{
    Q = 0,
    W,
    E,
    R,
    RockCyclone
}

[System.Serializable]
public class SkillStatus
{
    public string name;
    public string description;
    public SkillType skillType;
    public SkillStatus() { }

    public SkillType Type { get; set; }

    public SkillStatus(string name, string description, SkillType skillType)
    {
        this.name = name;
        this.description = description;
        this.Type = skillType;
    }

}

