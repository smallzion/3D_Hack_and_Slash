using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaider : MonoBehaviour
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
            enemy.onPlayerChaseInRange?.Invoke(other);
            Debug.Log("찾음");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.onPlayerChaseOutRange?.Invoke(other);
            Debug.Log("찾음");
        }
    }
}
