using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public override void Fire()
    {
        //base.Fire();
        var bullets = BulletPool.Instance.GetObject(5);

        // bullet spraying
        int angleStart = -15, angleDelta = 5, angleCurrent = angleStart;

        foreach (var bullet in bullets)
        {
            angleCurrent += angleDelta;
            bullet.transform.rotation = Quaternion.Euler(0, angleCurrent, 0);
            bullet.transform.position = Muzzle.position;
            BulletPool.Instance.PullObjectBack(bullet);
        }
    }
}
