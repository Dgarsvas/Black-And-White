using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardEnemy : BaseEnemy
{
    private Vector3 pos;
    private float lookRot;
    public void SetupEnemy(Vector3 pos, float lookRot)
    {
        this.pos = pos;
        this.lookRot = lookRot;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        entityDetector = gameObject.GetComponentInChildren<EntityDetector>();
        _stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody>();

        var guard = new Guard(this, navMeshAgent, animator, pos, lookRot);
        var search = new SearchForEntity(this, entityDetector, navMeshAgent, animator);
        var flee = new Flee(this, navMeshAgent, entityDetector, animator);
        var attack = new AttackEntity(this, navMeshAgent, entityDetector, animator);
        var die = new Die(this, navMeshAgent, entityDetector, animator);

        _stateMachine.AddAnyTransition(die, () => health <= 0);
        _stateMachine.AddTransition(guard, search, () => entityDetector.detected);
        _stateMachine.AddTransition(search, guard, () => search.timer >= GlobalAISettings.SEARCH_TIME);
        _stateMachine.AddTransition(search, attack, () => entityDetector.entity != null);
        _stateMachine.AddTransition(guard, attack, () => entityDetector.entity != null);

        //_stateMachine.AddAnyTransition(flee, () => CanRunAway());
        //_stateMachine.AddTransition(flee, patrol, () => RegainedCourage());

        _stateMachine.SetState(guard);
    }
}