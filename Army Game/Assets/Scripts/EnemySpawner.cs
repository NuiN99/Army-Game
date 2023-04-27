using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    Vector2 averageOfFriendlies;
    [SerializeField] GameObject currentEnemy;
    [SerializeField] float minSpawnRadius;
    [SerializeField] float maxSpawnRadius;
    [SerializeField] float spawnInterval;
    void Start()
    {
        StartCoroutine(SpawnAroundPlayer());
    }

    void Update()
    {
        
    }
    IEnumerator SpawnAroundPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            Vector2 randDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            Instantiate(currentEnemy, Vector2.zero/*WILL BE AVERAGE OF ALL FRIENDLIES*/ + randDir * UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius), Quaternion.identity);
        }
    }
}
