using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Girl : BaseEntity
{
    protected StateMachine _stateMachine;
    protected EntityDetector entityDetector;
    protected Rigidbody rb;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    public void Setup(Vector3 end)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        entityDetector = gameObject.GetComponentInChildren<EntityDetector>();
        _stateMachine = new StateMachine();

        var wait = new WaitAt(animator);
        var run = new RunTo(this, navMeshAgent, animator, end);

        _stateMachine.AddTransition(wait, run, () => entityDetector.detected);

        _stateMachine.SetState(run);
    }

    public override void TakeDamage(float damage, Vector3 direction)
    {
        //EMPTY
    }

    public override void Despawn()
    {
        base.Despawn();
    }
}
