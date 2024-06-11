using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float maxHp;

    private float hp;
    public float Hp
    {
        get => hp;
        private set
        {
            if(hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHp);
            }
            if(hp < 0)
            {
                Die();
            }

        }
    }

    public float damage;

    public float speed;

    private void OnEnable()
    {
        hp = maxHp;
    }


    public void TakeDamage(float damage)
    {
        Hp -= damage;
        Debug.Log("남은 체력" + Hp);
    }

    private void Die()
    {
    }
}
