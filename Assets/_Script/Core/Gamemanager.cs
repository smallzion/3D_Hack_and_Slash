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

    public Action<int> onWaveChange;
    int gameWave = 1;
    public int Wave
    {
        get => gameWave;
        private set
        {
            if (gameWave != value)
            {
                gameWave = value;
                onWaveChange?.Invoke(gameWave);
            }
        }
    }
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
    public Action onEnemyKillCountChange;
    public Action onEnemyDie;
    int enemyScore = 0;
    public int EnemyScore
    {
        get => enemyScore;
        private set
        {
            if(enemyScore != value)
            {
                enemyScore = value;
                onEnemyKillCountChange?.Invoke();
            }
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        LoadSkillData();
        onEnemyDie += ChangeScore;
    }

    private void ChangeScore()
    {
        EnemyScore++;
        if (EnemyScore % 10 == 0)
        {
            onSkillUpgrade?.Invoke();
        }
        if(EnemyScore % 100 == 0)
        {
            Wave++;
        }
        Debug.Log(EnemyScore);
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
#if UNITY_EDITOR
    public void TestEnemyKill(int enemyCount)
    {
        EnemyScore += enemyCount;

        if (EnemyScore % 100 == 0)
        {
            Wave++;
        }
        Debug.Log(EnemyScore);
    }
    private void TestChangeScore()
    {
        
    }
#endif
}
