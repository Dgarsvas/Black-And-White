using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackEntity : IState
{
    private BaseEntity _entity;
    private NavMeshAgent _navMeshAgent;
    private EntityDetector _enemyDetector;
    private Animator _animator;
    private Transform _enemy;
    private float attackDelay;

    public AttackEntity(BaseEntity entity, NavMeshAgent navMeshAgent, EntityDetector entityDetector, Animator animator)
    {
        _entity = entity;
        _navMeshAgent = navMeshAgent;
        _enemyDetector = entityDetector;
        _animator = animator;
        _enemy = entityDetector.entity;
    }

    public void OnEnter()
    {
        _enemy = _enemyDetector.entity;
        attackDelay =  (_entity as BaseEnemy).attackDelay;
    }

    public void OnExit()
    {
        //throw new System.NotImplementedException();
    }

    public void Tick()
    {
        attackDelay -= Time.deltaTime;
        if(_enemyDetector.hasSight && attackDelay<=0) _entity.AttackEnemy(_enemy);
    }
}
