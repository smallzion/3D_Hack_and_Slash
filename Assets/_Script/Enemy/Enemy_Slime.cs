using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_Slime : EnemyBase
{
    public float maxHp = 100f;
    public float hp = 100f;
    override public float Hp
    {
        get => hp;
        set
        {
            if (hp != value)
            {
                hp = Mathf.Clamp(value, 0, maxHp);
                if (hp < 0.1f)
                {
                    State = EnemyState.Die;
                }
            }
        }
    }
    GameObject playerRader;
    GameObject playerAttackRader;


    float attackPower = 5f;
    private Vector3 walkPoint;
    public float patrolRange = 10f; // 배회 범위
    NavMeshAgent agent;
    Player player;
    private float timer;
    public float patrolTime = 5f;

    Animator anim;

    float attackTimer = 1.0f;
    float elepsedTime = 0.0f;
    bool isDie = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        Refresh();
    }
    protected override void Awake()
    {
        hp = maxHp;
        playerRader = transform.GetChild(2).gameObject;
        playerAttackRader = transform.GetChild(3).gameObject;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
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
        anim.SetBool("IsWalk", true);
        anim.SetBool("IsRun", false);
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
        if (elepsedTime > attackTimer)
        {
            anim.SetFloat("AttackRandom", Random.Range(0, 1f));
            Collider[] targets = Physics.OverlapSphere(transform.position, 2.0f, LayerMask.GetMask("Player"));
            foreach (var target in targets)
            {
                if (target.gameObject.CompareTag("Player"))
                {
                    player.Defense(attackPower);
                }
            }
            elepsedTime = 0;
        }
        else
        {
            anim.SetFloat("AttackRandom", 0.5f);
            elepsedTime += Time.deltaTime;
        }
    }
    protected override void Update_Chase()
    {
        agent.SetDestination(player.transform.position);
        anim.SetBool("IsWalk", false);
        anim.SetBool("IsRun", true);

    }
    protected override void Update_Die()
    {
        if (!isDie)
        {
            anim.SetBool("IsWalk", false);
            anim.SetBool("IsRun", false);
            agent.isStopped = true;
            Die();
        }

    }
    protected override void Update()
    {
        base.Update();
    }
    public override void Defense(float attackPower)
    {
        if (!isDie)
        {
            Hp -= attackPower;
            anim.SetTrigger("GetHit");
        }
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
        if (!isDie)
        {
            isDie = true;
            playerRader.SetActive(false);
            playerAttackRader.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(DieAction());
        }
    }

    IEnumerator DieAction()
    {
        anim.SetBool("IsDie", true);
        yield return new WaitForSeconds(2.2f);
        GameManager.Instance.enemyKillCount?.Invoke();
        gameObject.SetActive(false);
        yield return null;
    }
    void Refresh()
    {
        isDie = false;
        Hp = maxHp;
        playerRader.SetActive(true);
        playerAttackRader.SetActive(true);
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

    void OnDrawGizmosSelected()
    {
        // 공격 범위를 기즈모로 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
    }
}