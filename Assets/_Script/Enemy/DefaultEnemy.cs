using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DefaultEnemy : EnemyBase
{
    public float hp = 100f;
    override public float Hp
    {
        get => hp; 
        set
        {
            if(hp != value)
            {
                hp = Mathf.Clamp(value,0,100);
                if(hp < 0.1f)
                {
                    State = EnemyState.Die;
                }
            }   
        }
    }
    NavMeshAgent agent;
    Player player;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void Start()
    {
        player = GameManager.Instance.Player;
        base.Start();
    }

    protected override void Update_Stop()
    {
    }
    protected override void Update_Patrol()
    {
    }
    protected override void Update_Attack()
    {
    }
    protected override void Update_Chase()
    {
        agent.SetDestination(player.transform.position);
    }
    protected override void Update_Die()
    {
        Die();
    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Defense(float attackPower)
    {
        Hp -= attackPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            State = EnemyState.Chase;
        }
    }
    public override void Die()
    {
        base.Die();
    }
}