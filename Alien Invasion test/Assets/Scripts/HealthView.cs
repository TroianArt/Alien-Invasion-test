using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Enemy enemy;
    [SerializeField] private float timeToHide = 0.5f;

    private float currentTime;
    
    private void Start()
    {
        SetActiveView(false);
        enemy.OnHealtChanged += OnEnemyHealtChanged;
        enemy.OnDeath += OnEnemyDeath;
    }

    private void OnEnemyHealtChanged(float health)
    {
        healthImage.fillAmount = health / enemy.MaxHealth;
        healthText.text = ((int)health).ToString();
        SetActiveView(true);
        currentTime = timeToHide;
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0) SetActiveView(false);
    }

    public void OnEnemyDeath(Enemy enemy)
    {
        SetActiveView(false);
    }
    
    private void OnDestroy()
    {
        enemy.OnHealtChanged -= OnEnemyHealtChanged;
        enemy.OnDeath -= OnEnemyDeath;
    }

    private void SetActiveView(bool isActive)
    {
        healthImage.gameObject.SetActive(isActive);
        healthText.gameObject.SetActive(isActive);
    }
}
