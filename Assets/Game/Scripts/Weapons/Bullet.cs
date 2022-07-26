using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Specs")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {      
            if(other.transform.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                BulletPool.Instance.PullObjectBackImmediate(this);
            }
        }
    }
}
