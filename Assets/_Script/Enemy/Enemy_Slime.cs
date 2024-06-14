using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Slime : EnemyBase
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
    private Vector3 walkPoint;
    public float patrolRange = 10f; // 배회 범위
    NavMeshAgent agent;
    Player player;
    private float timer;
    public float patrolTime = 5f;
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Awake();
    }

    protected override void Start()
    {
        player = GameManager.Instance.Player;
        onEnemyStateUpdate = Update_Patrol;
    }

    protected override void Update_Stop()
    {
    }
    protected override void Update_Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                SetNewRandomDestination();
                timer = patrolTime;
            }
        }
    }
    protected override void Update_Attack()
    {
        //공격 구현
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

    protected override void PlayerInChaseRange(Collider other)
    {
        base.PlayerInChaseRange(other);
    }

    protected override void PlayerOutChaseRange(Collider other)
    {
        base.PlayerOutChaseRange(other);
    }

    protected override void PlayerInAttackRange(Collider other)
    {
        base.PlayerInAttackRange(other);
    }
  
    protected override void PlayerOutAttackRange(Collider other)
    {
        base.PlayerOutAttackRange(other);
    }
    public override void Die()
    {
        base.Die();
    }
    void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRange;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRange, 1);
        walkPoint = hit.position;

        agent.SetDestination(walkPoint);
    }
}