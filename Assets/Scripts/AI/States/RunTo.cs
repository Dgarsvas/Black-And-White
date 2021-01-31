using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunTo : IState
{
    private readonly BaseEntity _entity;
    private NavMeshAgent _navMeshAgent;
    private readonly EntityDetector _enemyDetector;
    private Animator _animator;
    private Vector3 _destination;

    public RunTo(BaseEntity entity, NavMeshAgent navMeshAgent, Animator animator, Vector3 destination)
    {
        _entity = entity;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _destination = destination;

    }

    public void OnEnter()
    {
        Debug.Log("Girl runTo stair");
        _navMeshAgent.enabled = true;
        _navMeshAgent.destination = _destination;
        GameManager.instance.GirlFound();
    }

    public void Tick()
    {
        if (AIUtils.ApproximatePositionReached(_entity.transform.position, _destination))
        {
            _entity.Despawn();
        }
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
    }
}
