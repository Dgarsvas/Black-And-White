using System;
using UnityEngine;
using UnityEngine.AI;

internal class Patrol : IState
{
    private readonly Grunt _grunt;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private Vector3 _lastPosition = Vector3.zero;

    private Transform[] _points;
    private int currentPoint;

    public Patrol(Grunt grunt, NavMeshAgent navMeshAgent, Animator animator, Transform[] patrolPoints)
    {
        _grunt = grunt;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _points = patrolPoints;
    }
    
    public void Tick()
    {
        if (AIUtils.ApproximatePositionReached(_grunt.transform.position, _points[currentPoint].position))
        {
            GoToNextPoint();
        }

        _lastPosition = _grunt.transform.position;
    }

    private void GoToNextPoint()
    {
        if (currentPoint + 1 >= _points.Length)
        {
            currentPoint = 0;
        }
        else
        {
            currentPoint++;
        }

        _navMeshAgent.SetDestination(_points[currentPoint].position);
    }

    public void OnEnter()
    {
        currentPoint = 0;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_points[currentPoint].position);
        _animator.SetFloat(Speed, 1f);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetFloat(Speed, 0f);
    }
}