using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Die : IState
{
    private readonly Grunt _grunt;
    private NavMeshAgent _navMeshAgent;
    private readonly EntityDetector _enemyDetector;
    private Animator _animator;

    private float _initialSpeed;
    private float timer;

    public Die(Grunt grunt, NavMeshAgent navMeshAgent, EntityDetector enemyDetector, Animator animator)
    {
        _grunt = grunt;
        _navMeshAgent = navMeshAgent;
        _enemyDetector = enemyDetector;
        _animator = animator;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
        _enemyDetector.enabled = false;
        //TODO activate ragdoll
        _initialSpeed = _navMeshAgent.speed;
    }

    public void Tick()
    {
        if (timer < GlobalAISettings.DESPAWN_TIME)
        {
            timer += Time.deltaTime;
        }
        else
        {
            _grunt.Despawn();
        }
    }

    public void OnExit()
    {

    }
}