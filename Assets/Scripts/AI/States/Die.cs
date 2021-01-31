using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Die : IState
{
    private readonly BaseEntity _entity;
    private NavMeshAgent _navMeshAgent;
    private readonly EntityDetector _enemyDetector;
    private Animator _animator;

    private float timer;

    public Die(BaseEntity entity, NavMeshAgent navMeshAgent, EntityDetector enemyDetector, Animator animator)
    {
        _entity = entity;
        _navMeshAgent = navMeshAgent;
        _enemyDetector = enemyDetector;
        _animator = animator;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
        _enemyDetector.enabled = false;
        _animator.speed = 0;
        //TODO activate ragdoll
    }

    public void Tick()
    {
        if (timer < GlobalAISettings.DESPAWN_TIME)
        {
            timer += Time.deltaTime;
        }
        else
        {
            _entity.Despawn();
        }
    }

    public void OnExit()
    {

    }
}