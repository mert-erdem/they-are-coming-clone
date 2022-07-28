using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Specs")]
    [SerializeField] private int health = 1;
    [SerializeField] private float speed = 5f;

    private Transform target;

    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = health;
    }

    private void Start()
    {
        
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
        var newPos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        var lookOnLook = Quaternion.LookRotation(target.position - transform.position);
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
