using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitAt : IState
{
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    public WaitAt(NavMeshAgent navMeshAgent, Animator animator)
    {
        _navMeshAgent = navMeshAgent;
        _animator = animator;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = false;
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
