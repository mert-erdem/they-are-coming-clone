using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject Bullet;
    public Transform Muzzle;
    public float FireRate;
    private float LastFireTime = 0f;


    private void Update()
    {
        if (Time.time >= LastFireTime)
        {
            Fire();
            LastFireTime = Time.time + FireRate;
        }
    }

    public virtual void Fire()
    {
        var bullet = BulletPool.Instance.GetObject();
        bullet.transform.position = Muzzle.position;
        BulletPool.Instance.PullObjectBack(bullet);
    }
}
