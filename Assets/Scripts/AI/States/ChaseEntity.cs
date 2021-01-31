using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseEntity : IState
{
    private BaseEntity _entity;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private EntityDetector _entityDetector;
    private Transform _enemy;
    private Vector3 _target;
    private static readonly int Speed = Animator.StringToHash("Speed");
    public bool inPosition;
    public float timer;
    private float waitTimer;

    public ChaseEntity(BaseEntity entity, EntityDetector entityDetector, NavMeshAgent navMeshAgent, Animator animator)
    {
        _entity = entity;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _target = entityDetector.entityPos;
        _entityDetector = entityDetector;
    }

    public void Tick()
    {
        if (!inPosition)
        {
            if (AIUtils.ApproximatePositionReached(_entity.transform.position, _target))
            {
                inPosition = true;
            }
        }
        Debug.Log("Position:" + inPosition);
        _animator.SetBool("Walking", !inPosition);
        //_navMeshAgent.SetDestination(_target);
        _entity.transform.rotation = AIUtils.LookAt(_entity.transform.position, _target);
        //if (_entityDetector.hasSight) _grunt.AttackEnemy(_enemy);
        Debug.Log("Chasing player");
    }

    private float ApproximateDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

    public void OnEnter()
    {
        inPosition = false;
        _navMeshAgent.enabled = true;
        _target = _entityDetector.entityPos; // Vector3.Lerp( _grunt.transform.position, _entityDetector.entityPos, 0.0f);
        _enemy = _entityDetector.entity;
        _navMeshAgent.SetDestination(_target);
        _animator.SetFloat(Speed, 1f);
    }

    public void OnExit()
    {
        _entityDetector.canReactToNewDetections = true;
        _navMeshAgent.enabled = false;
        //_animator.SetFloat(Speed, 0f);
    }
}