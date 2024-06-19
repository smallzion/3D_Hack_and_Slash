using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_03_SkillLv : TestBase
{
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.IceSkillLvUp();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.IceSkillLvDown();
    }
}
