using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private Transform target, spawnBorderLeft, spawnBorderRight;
    [Header("Specs")]
    [SerializeField] private float spawnDeltaTime = 2f;
    private float lastTimeSpawned = 0f;

    private bool canSpawn = false;

    private void Start()
    {
        GameManager.ActionGameStart += TriggerCanSpawn;
        GameManager.ActionGameOver += TriggerCanSpawn;
        GameManager.ActionMiniGame += TriggerCanSpawn;
    }

    private void Update()
    {
        if(canSpawn && Time.time >= lastTimeSpawned)
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

    private void TriggerCanSpawn()
    {
        canSpawn = !canSpawn;
    }

    private void OnDestroy()
    {
        GameManager.ActionGameStart -= TriggerCanSpawn;
        GameManager.ActionGameOver -= TriggerCanSpawn;
        GameManager.ActionMiniGame -= TriggerCanSpawn;
    }
}
