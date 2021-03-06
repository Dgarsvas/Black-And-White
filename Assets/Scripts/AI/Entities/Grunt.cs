﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class GruntDep : BaseEnemy
{
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
        var chase = new ChaseEntity(this, entityDetector, navMeshAgent, animator);

        _stateMachine.AddAnyTransition(die, () => health <= 0);
        _stateMachine.AddTransition(patrol, search, () => entityDetector.detected);
        _stateMachine.AddTransition(search, patrol, () => search.timer >= GlobalAISettings.SEARCH_TIME);
        _stateMachine.AddTransition(search, attack, () => entityDetector.entity != null);
        _stateMachine.AddTransition(patrol, attack, () => entityDetector.entity != null);
        _stateMachine.AddTransition(attack, chase, () => entityDetector.entity != null && !entityDetector.hasSight);
        _stateMachine.AddTransition(chase, attack, () => entityDetector.entity != null && entityDetector.hasSight);
        _stateMachine.AddTransition(chase, search, () => entityDetector.entity == null || chase.inPosition);
        //_stateMachine.AddTransition(attack, search, () => entityDetector.entity == null);

        //_stateMachine.AddAnyTransition(flee, () => CanRunAway());
        //_stateMachine.AddTransition(flee, patrol, () => RegainedCourage());

        _stateMachine.SetState(patrol);
    }

    private void Update()
    {
        
        //entityDetector.hasSight = false;
    }
}