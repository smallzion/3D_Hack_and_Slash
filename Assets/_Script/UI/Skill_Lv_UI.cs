using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillLevelUpUI : MonoBehaviour
{
    Player player;
    public List<SkillStatus> skillStatuses; // 스킬 데이터를 저장할 리스트
    private TextMeshProUGUI[] skillTexts;
    List<int> skillCounts;
    private void Awake()
    {
        skillTexts = new TextMeshProUGUI[transform.childCount];
        for (int i = 0; i < skillTexts.Length; i++)
        {
            skillTexts[i] = transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();

            // EventTrigger 컴포넌트를 추가하고 클릭 이벤트 설정
            /*EventTrigger trigger = transform.GetChild(i).gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((eventData) => { OnSkillImageClick(eventData); });
            trigger.triggers.Add(entry);*/
        }

    }

    private void Start()
    {

        player = GameManager.Instance.Player;
        skillCounts = new();
        for (int i = 0; i < GameManager.Instance.SkillStatusData.Count; i++)
        {
            skillCounts.Add(i);
        }

        UpdateSkillTexts();
        gameObject.SetActive(false);
    }

    private void UpdateSkillTexts()
    {
        List<int> numbers = new List<int>();
        numbers.AddRange(skillCounts);
        System.Random rand = new System.Random();
        numbers = numbers.OrderBy(x => rand.Next()).ToList();
        int[] selectedNumbers = new int[3];
        selectedNumbers = numbers.Take(3).ToArray();

        // 스킬 상태 초기화
        skillStatuses.Clear();

        if (GameManager.Instance.SkillStatusData.Count >= 3)
        {
            for (int i = 0; i < selectedNumbers.Length; i++)
            {
                // selectedNumbers[i]가 SkillStatusData의 크기보다 작은지 확인
                if (selectedNumbers[i] < GameManager.Instance.SkillStatusData.Count)
                {
                    SkillStatus randomSkill = GameManager.Instance.GetRandomSkillData(selectedNumbers[i]);
                    skillStatuses.Add(randomSkill);
                    skillTexts[i].text = $"{randomSkill.name}\n\n\n{randomSkill.description}";
                }
                else
                {
                    Debug.LogWarning($"Index {selectedNumbers[i]} is out of range for SkillStatusData.");
                }
            }
        }
        else if (GameManager.Instance.SkillStatusData.Count == 2)
        {
            SkillStatus randomSkill1 = GameManager.Instance.GetRandomSkillData(0);
            SkillStatus randomSkill2 = GameManager.Instance.GetRandomSkillData(1);
            skillStatuses.Add(randomSkill1);
            skillStatuses.Add(randomSkill2);
            skillStatuses.Add(randomSkill2);

            skillTexts[0].text = $"{randomSkill1.name}\n\n\n{randomSkill1.description}";
            skillTexts[1].text = $"{randomSkill2.name}\n\n\n{randomSkill2.description}";
            skillTexts[2].text = $"{randomSkill2.name}\n\n\n{randomSkill2.description}";
        }
        else if (GameManager.Instance.SkillStatusData.Count == 1)
        {
            SkillStatus randomSkill = GameManager.Instance.GetRandomSkillData(0);
            skillStatuses.Add(randomSkill);
            skillStatuses.Add(randomSkill);
            skillStatuses.Add(randomSkill);

            skillTexts[0].text = $"{randomSkill.name}\n\n\n{randomSkill.description}";
            skillTexts[1].text = $"{randomSkill.name}\n\n\n{randomSkill.description}";
            skillTexts[2].text = $"{randomSkill.name}\n\n\n{randomSkill.description}";
        }
        else
        {
            // SkillStatusData가 비어 있는 경우
            for (int i = 0; i < skillTexts.Length; i++)
            {
                skillTexts[i].text = "No skill available";
            }
            Debug.LogWarning("SkillStatusData list is empty.");
        }
    }

    public void UpdateSkillUI()
    {
        if (GameManager.Instance.SkillStatusData.Count > 0)
        {
            gameObject.SetActive(true);
            UpdateSkillTexts();
        }
        else
        {
            gameObject.SetActive(false);
            Debug.LogWarning("SkillStatusData list is empty. Skill Level Up UI is not shown.");
        }
    }

    public void OnSkillImage0SlotClick()
    {

        SkillLevel(skillStatuses[0].skillType, 0);
        //GameManager.Instance.SkillStatusData.Remove(skillStatuses[0]);
        //skillCounts.RemoveAt(skillCounts.Count - 1);
        Debug.Log("0");
        skillStatuses.Clear();
        gameObject.SetActive(false);

    }

    public void OnSkillImage1SlotClick()
    {
        SkillLevel(skillStatuses[1].skillType, 1);
        //GameManager.Instance.SkillStatusData.Remove(skillStatuses[1]);
        //skillCounts.RemoveAt(skillCounts.Count - 1);

        Debug.Log("1");
        skillStatuses.Clear();
        gameObject.SetActive(false);

    }

    public void OnSkillImage2SlotClick()
    {
        SkillLevel(skillStatuses[2].skillType, 2);
        //GameManager.Instance.SkillStatusData.Remove(skillStatuses[2]);
        //skillCounts.RemoveAt(skillCounts.Count - 1);
        Debug.Log("2");
        skillStatuses.Clear();
        gameObject.SetActive(false);
    }

    void SkillLevel(SkillType type, int index)
    {
        switch (type)
        {
            case SkillType.Q:
                player.qSkillDamage += 10f; Debug.Log(skillStatuses[index]);
                GameManager.Instance.SkillStatusData.Remove(skillStatuses[index]);
                skillCounts.RemoveAt(skillCounts.Count - 1);
                break;
            case SkillType.W:
                player.wSkillDamage += 10f; Debug.Log(skillStatuses[index]);
                GameManager.Instance.SkillStatusData.Remove(skillStatuses[index]);
                skillCounts.RemoveAt(skillCounts.Count - 1);
                break;
            case SkillType.E:
                player.eSkillDamage += 1f; Debug.Log(skillStatuses[index]);
                GameManager.Instance.SkillStatusData.Remove(skillStatuses[index]);
                skillCounts.RemoveAt(skillCounts.Count - 1);
                break;
            case SkillType.R:
                player.rSkillActive = true; Debug.Log(skillStatuses[index]);
                GameManager.Instance.SkillStatusData.Remove(skillStatuses[index]);
                skillCounts.RemoveAt(skillCounts.Count - 1);
                break;
            case SkillType.RockCyclone:
                if(player.transform.GetChild(4).gameObject.activeSelf)
                {
                    RockCyclone iceCyclone = player.transform.GetChild(4).gameObject.GetComponent<RockCyclone>();
                    if(iceCyclone.SkillLevel < 3)
                    {
                        iceCyclone.SkillLevelUp();
                    }
                    else
                    {
                        iceCyclone.SkillLevelUp();
                        Debug.Log(skillStatuses[index]);
                        GameManager.Instance.SkillStatusData.Remove(skillStatuses[index]);
                        skillCounts.RemoveAt(skillCounts.Count - 1);
                    }
                }
                else
                {
                    player.transform.GetChild(4).gameObject.SetActive(true);
                }
                break;
        }
    }
}
