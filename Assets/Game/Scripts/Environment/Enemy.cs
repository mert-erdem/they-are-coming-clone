using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
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


    private void OnEnable()
    {
        currentHealth = health;
    }

    private void Update()
    {
        ChaseThePlayer();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void ChaseThePlayer()
    {
        var targetPos = target.position;
        targetPos.z -= 1.5f;
        var newPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        var lookOnLook = Quaternion.LookRotation(targetPos - transform.position);
        var newRot = Quaternion.Slerp(transform.rotation, lookOnLook, Time.deltaTime * 10f);

        transform.SetPositionAndRotation(newPos, newRot);
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
        EnemyPool.Instance.PullObjectBackImmediate(this);
    }
}
