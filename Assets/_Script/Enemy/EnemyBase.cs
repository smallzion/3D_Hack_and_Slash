using System;
using System.Collections;
using System.Collections.Generic;
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
    public Action<NavMeshAgent, int> onDebuffAttack;
    public Action onEnemyStateUpdate;
    public Action<EnemyState> onEnemyStateChange;

    public virtual float Hp { get; set; }


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
