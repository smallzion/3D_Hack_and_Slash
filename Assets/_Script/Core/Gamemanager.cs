using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;
    List<SkillStatus> skillStatusData = new List<SkillStatus>();
    public List<SkillStatus> SkillStatusData
    {
        get => skillStatusData;
        set
        {
            if(value != skillStatusData)
            {
                skillStatusData = value;
            }
        }
    }

    public Action onSkillUpgrade;
    public Action onEnemyKillCount;
    int enemyScore;
    public int EnemyScore => enemyScore;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        LoadSkillData();
        onEnemyKillCount += ChangeScore;
    }

    private void ChangeScore()
    {
        enemyScore++;
        if (enemyScore % 10 == 0)
        {
            onSkillUpgrade?.Invoke();
        }
        Debug.Log(enemyScore);
    }

    public void LoadSkillData()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("SkillData/SkillDatas");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(textAsset.text);
        
        XmlNodeList skillList = xmlDoc.SelectNodes("skills/skill");
        foreach (XmlNode skillInfo in skillList)
        {
            SkillStatus newSkill = new SkillStatus();
            newSkill.name = skillInfo.SelectSingleNode("name").InnerText;
            newSkill.description = skillInfo.SelectSingleNode("description").InnerText;
            string skillTypeStr = skillInfo.SelectSingleNode("skillType").InnerText;

            SkillType skillType;
            if (!Enum.TryParse(skillTypeStr, out skillType))
            {
                Debug.LogError($"Failed to parse skillType: {skillTypeStr}");
                continue;
            }
            newSkill.skillType = skillType;



            skillStatusData.Add(newSkill);
        }
    }
    public SkillStatus GetSkillData(SkillType skillType)
    {
        SkillStatus skill = skillStatusData.Find(skill => skill.Type == skillType);
        return skill;
    }

    public SkillStatus GetRandomSkillData(int index)
    {

        return skillStatusData[index];
    }
    /*public void PrintSkillData()
    {
        foreach (SkillStatus skill in skillStatusData)
        {
            Debug.Log("스킬 이름: " + skill.name + "스킬 설명: " + skill.description + "스킬 타입: " + skill.Type);
        }
    }*/

}
