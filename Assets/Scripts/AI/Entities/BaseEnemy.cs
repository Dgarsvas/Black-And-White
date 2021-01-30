using System;
using UnityEngine;
using UnityEngine.AI;

public enum Signs
{
    Searching,
    Spotted,
    Surrender
}

public class BaseEnemy : BaseEntity
{
    protected StateMachine _stateMachine;
    protected EntityDetector entityDetector;
    protected Rigidbody rb;
    protected NavMeshAgent navMeshAgent;
    protected Animator animator;

    public virtual void ShowSign(Signs sign)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        _stateMachine?.Tick();
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
