using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SearchForEntity : IState
{
    private readonly Grunt _grunt;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private EntityDetector _entityDetector;
    private Vector3 _target;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool inPosition;
    public float timer;
    private float waitTimer;

    public SearchForEntity(Grunt grunt, EntityDetector entityDetector, NavMeshAgent navMeshAgent, Animator animator)
    {
        _grunt = grunt;
        _navMeshAgent = navMeshAgent;
        _animator = animator;
        _target = entityDetector.entityPos;
        _entityDetector = entityDetector;
    }

    public void Tick()
    {
        if (waitTimer <= 0.3f)
        {
            waitTimer += Time.deltaTime;
            return;
        }

        if (!inPosition)
        {
            if (AIUtils.ApproximatePositionReached(_grunt.transform.position, _target))
            {
                inPosition = true;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private float ApproximateDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

    public void OnEnter()
    {
        _entityDetector.detected = false;
        inPosition = false;
        _navMeshAgent.enabled = true;
        _navMeshAgent.SetDestination(_target);
        _animator.SetFloat(Speed, 1f);
    }

    public void OnExit()
    {
        _entityDetector.canReactToNewDetections = true;
        _navMeshAgent.enabled = false;
        _animator.SetFloat(Speed, 0f);
    }
}