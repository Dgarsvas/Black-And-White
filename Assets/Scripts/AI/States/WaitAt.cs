using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitAt : IState
{
    private Animator _animator;

    public WaitAt(Animator animator)
    {
        _animator = animator;
    }

    public void OnEnter()
    {
    }

    public void Tick()
    {
    }

    public void OnExit()
    {
    }
}
