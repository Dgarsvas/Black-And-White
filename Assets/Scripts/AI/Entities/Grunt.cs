using System;
using UnityEngine;
using UnityEngine.AI;

public class Grunt : BaseEnemy
{
    public float speed;
    public float waitTime;
    private StateMachine _stateMachine;
    private EntityDetector entityDetector;
    public Weapon equipedWeapon;

    [SerializeField]
    private Transform[] patrolPoints;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        var animator = GetComponent<Animator>();
        entityDetector = gameObject.GetComponentInChildren<EntityDetector>();

        _stateMachine = new StateMachine();

        var patrol = new Patrol(this, navMeshAgent, animator, patrolPoints);
        var search = new SearchForEntity(this, entityDetector, navMeshAgent, animator);
        var flee = new Flee(this, navMeshAgent, entityDetector, animator);
        var attack = new AttackEntity(this, navMeshAgent, entityDetector, animator);

        _stateMachine.AddTransition(patrol, search, () => SpottedEnemy());
        _stateMachine.AddTransition(search, patrol, () => search.timer >= waitTime);
        _stateMachine.AddTransition(search, attack, () => SeeEnemy());
        _stateMachine.AddTransition(patrol, attack, () => SeeEnemy());

        //_stateMachine.AddAnyTransition(flee, () => CanRunAway());
        //_stateMachine.AddTransition(flee, patrol, () => RegainedCourage());

        _stateMachine.SetState(patrol);
    }

    private bool RegainedCourage()
    {
        throw new NotImplementedException();
    }

    private bool CanRunAway()
    {
        return false;
    }

    private bool SeeEnemy()
    {
        return entityDetector.entity != null;
    }

    private bool SpottedEnemy()
    {
        return entityDetector.detected;
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public override void TakeDamage(float damage)
    {
        health = Math.Max(0, health - damage);
    }

    public void AttackEnemy(Transform enemy) {
        Vector3 direction = (transform.position - enemy.position).normalized;
        transform.rotation = AIUtils.RotateY(direction);
        equipedWeapon.Shoot(new Vector3());
    }
}