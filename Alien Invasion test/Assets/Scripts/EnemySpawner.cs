using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int count;
    [SerializeField] private float intervalTime;
    [SerializeField] private float size;
    
    private float timer;
    private int currentCount;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > intervalTime && count > currentCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.OnDeath += OnDeath;
        enemy.Init(transform.position, size);
        currentCount++;
        timer = 0;
    }

    private void OnDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnDeath;

        currentCount--;
    }
}
