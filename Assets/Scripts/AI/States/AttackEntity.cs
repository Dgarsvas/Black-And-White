using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackEntity : IState
{
    private Grunt _gatherer;
    private NavMeshAgent _navMeshAgent;
    private EntityDetector _enemyDetector;
    private Animator _animator;
    private Transform _enemy;
    private float attackDelay;

    public AttackEntity(Grunt grunt, NavMeshAgent navMeshAgent, EntityDetector entityDetector, Animator animator)
    {
        _gatherer = grunt;
        _navMeshAgent = navMeshAgent;
        _enemyDetector = entityDetector;
        _animator = animator;
        _enemy = entityDetector.entity;
    }

    public void OnEnter()
    {
        _enemy = _enemyDetector.entity;
        attackDelay = _gatherer.attackDelay;
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public void Tick()
    {
        attackDelay -= Time.deltaTime;
        if(_enemyDetector.hasSight && attackDelay<=0) _gatherer.AttackEnemy(_enemy);
    }
}
