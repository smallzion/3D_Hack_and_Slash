using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCyclone : MonoBehaviour
{
    Transform[] iceSpears;
    int skillLevel = 0;
    public int SkillLevel => skillLevel;
    public int speed = 1;
    
    Player player;

    private void Awake()
    {
        iceSpears = new Transform[transform.childCount];
        player = transform.parent.GetComponent<Player>();
        player.IceSkillLvUp += SkillLevelUp;
        player.IceSkillLvDown += SkillLevelDown;
        for (int i = 0; i < iceSpears.Length; i++)
        {
            iceSpears[i] = transform.GetChild(i);
            iceSpears[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Rotate(0, speed, 0);
    }

    public void SkillLevelUp()
    {
        skillLevel++;
        SelectSkillLV();
    }

    public void SkillLevelDown()
    {
        skillLevel--;
        SelectSkillLV();
    }
    
    void SelectSkillLV()
    {
        // 모든 iceSpears를 비활성화
        for (int i = 0; i < iceSpears.Length; i++)
        {
            iceSpears[i].gameObject.SetActive(false);
        }

        // skillLevel에 따라 필요한 개수만큼 활성화
        for (int i = 0; i < skillLevel && i < iceSpears.Length; i++)
        {
            iceSpears[i].gameObject.SetActive(true);
        }

    }
}
