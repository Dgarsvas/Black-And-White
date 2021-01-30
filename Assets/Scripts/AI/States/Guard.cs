using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guard : IState
{
    private readonly BaseEntity _entity;
    private readonly NavMeshAgent _navMeshAgent;
    private readonly Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");

    private Vector3 _guardPos;
    private float _lookRot;
    private bool posReached;
    private float timer;

    public Guard(BaseEntity entity, NavMeshAgent navMeshAgent, Animator animator, Vector3 guardPos, float lookRot)
    {
        _entity = entity;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _guardPos = guardPos;
        _lookRot = lookRot;
    }

    public void Tick()
    {
        if (!posReached)
        {
            if (AIUtils.ApproximatePositionReached(_entity.transform.position, _guardPos))
            {
                posReached = true;
                _entity.transform.rotation = Quaternion.Euler(0f, _lookRot, 0f);
            }
        }
        else
        {
            _entity.transform.rotation = Quaternion.Euler(0f, _lookRot + Mathf.Sin(timer) * GlobalAISettings.LOOK_DEGREES, 0f);
            timer += Time.deltaTime;
        }
        
    }

    public void OnEnter()
    {
        posReached = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_guardPos);
        _animator.SetFloat(Speed, 1f);
    }

    public void OnExit()
    {
        _navMeshAgent.enabled = false;
        _animator.SetFloat(Speed, 0f);
    }
}
