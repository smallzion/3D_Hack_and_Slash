using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCyclone : MonoBehaviour
{
    Transform[] rocks;
    int skillLevel = 0;
    public int SkillLevel => skillLevel;
    public int speed = 1;
    public float radius = 5f;

    Player player;

    private void Awake()
    {
        rocks = new Transform[transform.childCount];
        player = transform.parent.GetComponent<Player>();
        player.IceSkillLvUp += SkillLevelUp;
        player.IceSkillLvDown += SkillLevelDown;
        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i] = transform.GetChild(i);
            rocks[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

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
        for (int i = 0; i < rocks.Length; i++)
        {
            rocks[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < skillLevel && i < rocks.Length; i++)
        {
            rocks[i].gameObject.SetActive(true);

            if (i == 0)
            {
                rocks[i].localPosition = new Vector3(0, 0, radius);
            }
            else
            {
                float angle = 360f / skillLevel * i;
                float radians = angle * Mathf.Deg2Rad;
                float x = Mathf.Sin(radians) * radius;
                float z = Mathf.Cos(radians) * radius;
                rocks[i].localPosition = new Vector3(x, 0, z);
            }
        }
    }
}
