using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            enemy.onPlayerAttackInRange?.Invoke(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.onPlayerAttackOutRange?.Invoke(other);
        }
    }
}
