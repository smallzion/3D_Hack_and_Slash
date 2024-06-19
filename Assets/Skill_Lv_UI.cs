using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelUpUI : MonoBehaviour
{
    public List<SkillStatus> skillStatuses; // 스킬 데이터를 저장할 리스트
    private TextMeshProUGUI[] skillTexts;

    private void Awake()
    {
        skillTexts = new TextMeshProUGUI[transform.childCount];
        for (int i = 0; i < skillTexts.Length; i++)
        {
            skillTexts[i] = transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();

            // EventTrigger 컴포넌트를 추가하고 클릭 이벤트 설정
            EventTrigger trigger = transform.GetChild(i).gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnSkillImageClick(eventData); });
            trigger.triggers.Add(entry);
        }
    }

    private void OnEnable()
    {
        UpdateSkillTexts();
    }

    private void UpdateSkillTexts()
    {
        for (int i = 0; i < skillTexts.Length; i++)
        {
            SkillStatus randomSkill = GetRandomSkill();
            skillTexts[i].text = $"{randomSkill.name}\n{randomSkill.description}";
        }
    }

    private SkillStatus GetRandomSkill()
    {
        if (skillStatuses == null || skillStatuses.Count == 0)
        {
            return new SkillStatus("Unknown", "No Description", SkillType.Fire);
        }

        int randomIndex = Random.Range(0, skillStatuses.Count);
        return skillStatuses[randomIndex];
    }

    public void OnSkillImageClick(BaseEventData eventData)
    {
        for (int i = 0; i < skillTexts.Length; i++)
        {
            if (eventData.selectedObject == skillTexts[i].transform.parent.gameObject)
            {
                Debug.Log("Skill Image clicked: " + i);
                skillTexts[i].text = "Skill Level Up!";
                break;
            }
        }
    }
}
