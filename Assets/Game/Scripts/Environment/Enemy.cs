using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Specs")]
    [SerializeField] private int health = 1;
    [SerializeField] private float speed = 5f;

    private Transform target;
    
    private void Start()
    {
        target = HiveManager.Instance.transform;
    }

    private void Update()
    {
        ChaseThePlayer();
    }

    private void ChaseThePlayer()
    {
        transform.LookAt(target, Vector3.up);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        // animation etc.
        // object pool for enemies
        Destroy(gameObject);
    }
}
