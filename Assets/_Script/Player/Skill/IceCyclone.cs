using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCyclone : MonoBehaviour
{
    Transform[] iceSpears;
    int skillLevel = 0;
    public int speed = 1;

    private void Awake()
    {
        iceSpears = new Transform[transform.childCount];

        for (int i = 0; i < iceSpears.Length; i++)
        {
            iceSpears[i] = transform.GetChild(i);
            iceSpears[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        switch (skillLevel)
        {
            case 0:
                break;
            case 1:
                iceSpears[0].gameObject.SetActive(true);
                break;
            case 2:
                iceSpears[1].gameObject.SetActive(true);
                break;
            case 3:
                iceSpears[2].gameObject.SetActive(true);
                break;
            case 4:
                iceSpears[3].gameObject.SetActive(true);
                break;
        }
        transform.Rotate(0, speed, 0);
    }

    public void SkillLevelUp()
    {
        skillLevel++;
    }

    public void SkillLevelDown()
    {
        skillLevel--;
    }
}
