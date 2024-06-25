using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : RecycleObject, IHealth, IBattler
{
    public enum EnemyState
    {
        Stop = 0,
        Patrol,
        Chase,
        Attack,
        Die,
    }

    EnemyState state = EnemyState.Stop;

    public EnemyState State
    {
        get => state;
        protected set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case EnemyState.Stop:
                        Debug.Log("정지 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Stop;
                        break;
                    case EnemyState.Patrol:
                        Debug.Log("정찰 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        Debug.Log("추적 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        Debug.Log("공격 상태");
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Die:
                        Debug.Log("사망 상태");
                        onDie?.Invoke();
                        onEnemyStateChange?.Invoke(state);
                        onEnemyStateUpdate = Update_Die;

                        break;
                }
            }
        }
    }

    public Action onDie;
    public Action onEnemyStateUpdate;
    public Action<EnemyState> onEnemyStateChange;
    public Action<Collider> onPlayerChaseInRange;
    public Action<Collider> onPlayerChaseOutRange;
    public Action<Collider> onPlayerAttackInRange;
    public Action<Collider> onPlayerAttackOutRange;

    public virtual float Hp { get; set; }


    protected virtual void Awake()
    {
        onPlayerChaseInRange += PlayerInChaseRange;
        onPlayerChaseOutRange += PlayerOutChaseRange;
        onPlayerAttackInRange += PlayerInAttackRange;
        onPlayerAttackOutRange += PlayerOutAttackRange;
    }

    protected virtual void PlayerInChaseRange(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            State = EnemyState.Chase;
            Debug.Log("플레이어 찾음");
        }
    }

    protected virtual void PlayerOutChaseRange(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            State = EnemyState.Patrol;
            Debug.Log("플레이어 놓침");
        }
    }

    protected virtual void PlayerInAttackRange(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            State = EnemyState.Attack;
            Debug.Log("공격범위 안으로들어옴");
        }
    }

    protected virtual void PlayerOutAttackRange(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            State = EnemyState.Chase;
            Debug.Log("공격범위 밖으로나감");
        }
    }

    protected virtual void Start()
    {
        onEnemyStateUpdate = Update_Stop;
    }

    protected virtual void Update()
    {
        onEnemyStateUpdate();
    }

    protected virtual void Update_Stop()
    {

    }

    protected virtual void Update_Patrol()
    {

    }

    protected virtual void Update_Chase()
    {

    }

    protected virtual void Update_Attack()
    {

    }

    protected virtual void Update_Die()
    {

    }


    public virtual void Die()
    {
    }

    public virtual void Attack(IBattler target)
    {

    }

    public virtual void Defense(float attackPower)
    {

    }


}
