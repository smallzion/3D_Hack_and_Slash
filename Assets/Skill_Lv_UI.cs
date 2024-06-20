using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelUpUI : MonoBehaviour
{
    public List<SkillStatus> skillStatuses; // 스킬 데이터를 저장할 리스트
    private TextMeshProUGUI[] skillTexts;
    int[] skillCounts;
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

    private void Start()
    {
        skillCounts = new int[GameManager.Instance.SkillStatusData.Count];
        for(int i = 0;i < skillCounts.Length;i++)
        {
            skillCounts[i] = i; 
        }
        UpdateSkillTexts();
    }

    private void UpdateSkillTexts()
    {
        List<int> numbers = new List<int>();
        numbers.AddRange(skillCounts);
        System.Random rand = new System.Random();
        numbers = numbers.OrderBy(x => rand.Next()).ToList();
        int[] selectedNumbers = new int[3];
        selectedNumbers = numbers.Take(3).ToArray();

        for (int i = 0; i < selectedNumbers.Length; i++)
        {
            SkillStatus randomSkill = GameManager.Instance.GetRandomSkillData(selectedNumbers[i]);
            skillTexts[i].text = $"{randomSkill.name}\n{randomSkill.description}";
        }

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
