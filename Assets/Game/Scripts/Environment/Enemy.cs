using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private NavMeshAgent agent;

    [Header("Specs")]
    [SerializeField] private int health = 1;
    public int Health
    {
        set { this.health = value; }
        get { return health; }
    }
    [SerializeField] private float speed = 5f;
    public float Speed
    {
        set { this.speed = value; }
        get { return speed; }
    }

    private Transform target;
    private int currentHealth;


    private void Update()
    {
        if (target == null) return;
        
        ChaseThePlayer();
    }

    public void OnSpawn()
    {
        currentHealth = health;
        EnemyHiveManager.Instance.Join(this);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Stop()
    {
        target = null;
        rigidbody.isKinematic = true;
        agent.isStopped = true;
        animator.SetBool("Idle", true);
    }

    private void ChaseThePlayer()
    {
        agent.SetDestination(target.position);

        //var targetPos = target.position;
        //targetPos.z -= 1.5f;
        //var newPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //var lookOnLook = Quaternion.LookRotation(targetPos - transform.position);
        //var newRot = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);
        //
        //transform.SetPositionAndRotation(newPos, newRot);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // animation etc.
        // object pool for enemies
        EnemyHiveManager.Instance.Leave(this);
    }
}
