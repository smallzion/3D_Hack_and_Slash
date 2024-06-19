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
        LoadSkillData(); // 게임 매니저 초기화 시 스킬 데이터 로드
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
            Debug.LogError("SkillStatus.json 파일을 찾을 수 없습니다.");
        }
    }

    public SkillStatus GetSkillStatus(SkillType type)
    {
        return skillList.Find(skill => skill.skillType == type);
    }
}
