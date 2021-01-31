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
    public float attackDelay = 0.3f;

    public AudioClip hitSound;

    public virtual void ShowSign(Signs sign)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        _stateMachine?.Tick();
        if (entityDetector.entity != null) entityDetector.hasSight = entityDetector.DirectSight(entityDetector.entity.position);
        else entityDetector.hasSight = false;
        if (!entityDetector.hasSight) entityDetector.entity = null;
    }

    public override void TakeDamage(float damage, Vector3 dir)
    {
        health -= damage;

        if (health <= 0)
        {
            rb.isKinematic = false;
            rb.AddForce(dir * 500f);
        }
    }

    public override void Despawn()
    {
        Destroy(gameObject);
    }
}
