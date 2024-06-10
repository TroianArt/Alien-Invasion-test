using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float radius = 5;
    [SerializeField] private Animator animator;


    private List<Enemy> enemies = new List<Enemy>();
    private List<LineRenderer> lines = new List<LineRenderer>();

    private bool IsMaxEnemies => maxEnemies <= enemies.Count;

    private void Start()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            LineRenderer line = Instantiate(linePrefab);
            line.positionCount = 2;
            lines.Add(line);
            line.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemies.Count; i++) 
        {
            DrawLine(i, enemies[i].gameObject.transform.position);
            enemies[i].ChangeHP(-damage * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy) && !IsMaxEnemies)
        {
            AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Enemy>(out var enemy))
        {
            RemoveEnemy(enemy);
        }
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        RemoveEnemy(enemy);
        EnemyDeathAnimation(enemy);
    }

    private void AddEnemy(Enemy enemy)
    {
        if (IsMaxEnemies) return;
        enemies.Add(enemy);
        enemy.IsDamaging = true;
        enemy.OnDeath += OnEnemyDeath;
    }
    
    private void RemoveEnemy(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        enemies.Remove(enemy);
        enemy.IsDamaging = false;
        FindEnemy();
        lines.ForEach(x=>x.gameObject.SetActive(false));
    }

    private void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        if(colliders.Length == 0) return;

        foreach (var collider in colliders)
        {
            if(IsMaxEnemies) return;
            if (!collider.TryGetComponent<Enemy>(out var enemy) || enemies.Contains(enemy)
                || !enemy.IsAlive) continue;
            
            AddEnemy(enemy);
        }
    }

    private void DrawLine(int index, Vector3 enemyPostion)
    {
        lines[index].SetPosition(0, transform.position + new Vector3(0, 1.5f, 0));
        lines[index].SetPosition(1, enemies[index].gameObject.transform.position + new Vector3(0, 1.5f, 0));
        lines[index].gameObject.SetActive(true);
    }

    private void EnemyDeathAnimation(Enemy enemy)
    {
        Quaternion rotation = enemy.gameObject.transform.rotation;
        enemy.gameObject.transform.DOMoveX(transform.position.x, 0.5f);
        enemy.gameObject.transform.DOMoveZ(transform.position.z, 0.5f);
        enemy.gameObject.transform.DORotate(new Vector3(90, rotation.y, rotation.z), 0.5f);
        enemy.gameObject.transform.DOMoveY(4, 0.5f).OnComplete(() =>
        {
            enemy.gameObject.transform.DOScale(Vector3.zero, 0.3f);
            enemy.gameObject.transform.DOMove(transform.position + new Vector3(), 0.3f).OnComplete((() =>
            {
                Destroy(enemy.gameObject);
            }));
        });
    }
}
