using System;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : BaseEnemy
{
    private StateMachine _stateMachine;
    private EntityDetector entityDetector;
    private Rigidbody rb;

    [SerializeField]
    private Transform[] patrolPoints;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        entityDetector = gameObject.GetComponentInChildren<EntityDetector>();

        _stateMachine = new StateMachine();

        var patrol = new Patrol(this, navMeshAgent, animator, patrolPoints);
        var search = new SearchForEntity(this, entityDetector, navMeshAgent, animator);
        var flee = new Flee(this, navMeshAgent, entityDetector, animator);
        var attack = new AttackEntity(this, navMeshAgent, entityDetector, animator);
        var die = new Die(this, navMeshAgent, entityDetector, animator);

        _stateMachine.AddAnyTransition(die, () => health <= 0);
        _stateMachine.AddTransition(patrol, search, () => entityDetector.detected);
        _stateMachine.AddTransition(search, patrol, () => search.timer >= GlobalAISettings.SEARCH_TIME);
        _stateMachine.AddTransition(search, attack, () => entityDetector.entity != null);
        _stateMachine.AddTransition(patrol, attack, () => entityDetector.entity != null);

        //_stateMachine.AddAnyTransition(flee, () => CanRunAway());
        //_stateMachine.AddTransition(flee, patrol, () => RegainedCourage());

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public override void TakeDamage(float damage, Vector3 dir)
    {
        health -= damage;

        if (health <= 0)
        {
            rb.AddForce(dir * 500f);
        }
    }

    public override void Despawn()
    {
        Destroy(gameObject);
    }
}