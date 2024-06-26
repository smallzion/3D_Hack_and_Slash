using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_CoolTime : TestBase
{
#if UNITY_EDITOR
    Player player;


    void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player.TestCoolTimeRefresh();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDefaultEnemy();
    }
#endif
}
