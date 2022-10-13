using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private Transform spawnBorderLeft, spawnBorderRight;
    [Header("Specs")]
    [SerializeField] private float spawnDeltaTime = 2f;
    private float lastTimeSpawned = 0f;
    private bool canSpawn = false;


    private void Start()
    {
        GameManager.ActionGameStart += StartSpawn;
        GameManager.ActionGameOver += StopSpawn;
        GameManager.ActionMiniGame += StopSpawn;
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
        // change rotation
        var enemyRot = enemy.transform.rotation;
        enemyRot.y = 180f;
        enemy.transform.rotation = enemyRot;

        enemy.OnSpawn();

        //TODO: Spawn enemies randomly in different types
    }

    private void StartSpawn()
    {
        canSpawn = true;
    }

    private void StopSpawn()
    {
        canSpawn = false;
    }

    private void OnDestroy()
    {
        GameManager.ActionGameStart -= StartSpawn;
        GameManager.ActionGameOver -= StopSpawn;
        GameManager.ActionMiniGame -= StopSpawn;
    }
}
