using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_R : MonoBehaviour
{
    public float Damage = 50.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyBase>() != null)
            {
                other.GetComponent<EnemyBase>().Defense(Damage);
            }
        }
    }
}
