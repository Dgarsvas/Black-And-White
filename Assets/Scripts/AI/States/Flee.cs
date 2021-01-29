using UnityEngine;
using UnityEngine.AI;

public class Flee : IState
{
    private readonly Grunt _gatherer;
    private NavMeshAgent _navMeshAgent;
    private readonly EntityDetector _enemyDetector;
    private Animator _animator;
    private static readonly int FleeHash = Animator.StringToHash("Flee");

    private float _initialSpeed;
    private const float FLEE_SPEED = 6F;
    private const float FLEE_DISTANCE = 5F;

    public Flee(Grunt gatherer, NavMeshAgent navMeshAgent, EntityDetector enemyDetector, Animator animator)
    {
        _gatherer = gatherer;
        _navMeshAgent = navMeshAgent;
        _enemyDetector = enemyDetector;
        _animator = animator;
    }

    public void OnEnter()
    {
        _navMeshAgent.enabled = true;
        _animator.SetBool(FleeHash, true);
        _initialSpeed = _navMeshAgent.speed;
        _navMeshAgent.speed = FLEE_SPEED;
    }

    public void Tick()
    {
        if (_navMeshAgent.remainingDistance < 1f)
        {
            var away = GetRandomPoint();
            _navMeshAgent.SetDestination(away);
        }
    }

    private Vector3 GetRandomPoint()
    {
        //TODO
        return _gatherer.transform.position;
    }

    public void OnExit()
    {
        _navMeshAgent.speed = _initialSpeed;
        _navMeshAgent.enabled = false;
        _animator.SetBool(FleeHash, false);
    }
}