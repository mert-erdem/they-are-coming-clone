using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private Transform target, spawnBorderLeft, spawnBorderRight;
    [Header("Specs")]
    [SerializeField] private float spawnDeltaTime = 2f;
    private float lastTimeSpawned = 0f;


    private void Update()
    {
        if(Time.time >= lastTimeSpawned)
        {
            SpawnEnemy();
            lastTimeSpawned = Time.time + spawnDeltaTime;
        }
    }

    private void SpawnEnemy()
    {
        var randomX = Random.Range(spawnBorderLeft.position.x, spawnBorderRight.position.x);
        var spawnPos = new Vector3(randomX, 0f, transform.position.z);
        // object pool
        var enemy = EnemyPool.Instance.GetObject(typeof(BasicEnemy));
        enemy.transform.position = spawnPos;
        enemy.SetTarget(target);
        // change rotation
        var enemyRot = enemy.transform.rotation;
        enemyRot.y = 180f;
        enemy.transform.rotation = enemyRot;

        //TODO: Spawn enemies randomly in different types 
    }
}
