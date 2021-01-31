using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Girl : BaseEntity
{
    protected StateMachine _stateMachine;
    protected Rigidbody rb;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    private bool setupIsDone;
    private bool goingToStairs;
    public void Setup(Vector3 end)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        setupIsDone = true;
    }

    private void Update()
    {
        if (setupIsDone)
        {
            if (!goingToStairs)
            {
                if (AIUtils.ApproximatePositionReached(transform.position, GameManager.instance.playerTransform.position, 2f))
                {
                    GameManager.instance.GirlFound();
                    navMeshAgent.destination = GameManager.instance.stairsPos;
                    goingToStairs = true;
                }
            }
            else if(AIUtils.ApproximatePositionReached(transform.position, GameManager.instance.stairsPos, 2f))
            {
                Despawn();
            }

        }
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
