using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_07_WaveChange : TestBase
{

    public int enemyCount = 50;
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.TestEnemyKill(enemyCount);
        Debug.Log(GameManager.Instance.EnemyScore);
    }
}
