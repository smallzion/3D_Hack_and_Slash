using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_02_Enemy : TestBase
{
    Player player;


    void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDefaultEnemy();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.transform.GetChild(4).GetComponent<IceCyclone>().SkillLevelUp();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.transform.GetChild(4).GetComponent<IceCyclone>().SkillLevelDown();
    }
}
