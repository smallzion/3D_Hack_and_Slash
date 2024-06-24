using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class test_04_Skilllv_UI : TestBase
{
    public SkillLevelUpUI skillLevelUpUI;
    private void Start()
    {
        Transform parent = FindAnyObjectByType<Canvas>().transform;
        Transform child = parent.GetChild(0);
        skillLevelUpUI = child.GetComponent<SkillLevelUpUI>();
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        skillLevelUpUI.UpdateSkillUI();
    }
}
