using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameManager : Singleton<GameManager>
{
    Player player;
    List<SkillStatus> skillList;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        LoadSkillData(); // ���� �Ŵ��� �ʱ�ȭ �� ��ų ������ �ε�
    }

    void LoadSkillData()
    {
        string filePath = Path.Combine(Application.dataPath, "JsonFile", "SkillStatus.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            skillList = JsonUtility.FromJson<List<SkillStatus>>(json);
        }
        else
        {
            Debug.LogError("SkillStatus.json ������ ã�� �� �����ϴ�.");
        }
    }

    public SkillStatus GetSkillStatus(SkillType type)
    {
        return skillList.Find(skill => skill.skillType == type);
    }
}
