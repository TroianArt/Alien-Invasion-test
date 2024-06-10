using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public event Action<float> OnHealtChanged;
    public event Action<Enemy> OnDeath;
    
    [SerializeField] private float healthValue;
    [SerializeField] private float speed;
    [SerializeField] private float regeneration;
    [SerializeField] private Transform body;
    [SerializeField] private Animator animator;

    private float currentHealth;
    private Vector3 targetPoint;
    private Vector3 centerPosition;
    private float spawnerSize;
    private bool isAlive = true;
    
    public float MaxHealth => healthValue;
    public bool IsMaxHealth => currentHealth >= healthValue;
    public bool IsAlive => isAlive;
    public bool IsDamaging { get; set; }
    
    private void Start()
    {
        currentHealth = healthValue;
    }

    public void Init(Vector3 spawnerPosition, float size)
    {
        centerPosition = spawnerPosition;
        spawnerSize = size;
        RandomTarget();
    }

    private void RandomTarget()
    {
        float x = Random.Range(centerPosition.x - spawnerSize, centerPosition.x + spawnerSize);
        float z = Random.Range(centerPosition.z - spawnerSize, centerPosition.z + spawnerSize);
        targetPoint = new Vector3(x, 0, z);
    }
    
    private void Update()
    {
        if(!isAlive) return;
        
        body.LookAt(body.transform.position + targetPoint - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPoint) < 0.01f)
        {
            RandomTarget();
        }

        if (!IsDamaging && !IsMaxHealth)
        {
            ChangeHP(regeneration * Time.deltaTime);
        }
    }

    public void ChangeHP(float delta)
    {
        currentHealth += delta;
        currentHealth = Math.Clamp(currentHealth, 0, healthValue);
        OnHealtChanged.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            animator.enabled = false;
            isAlive = false;
            OnDeath.Invoke(this);
        }
    }
}
