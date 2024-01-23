using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{

    public Slider slider;
    public Unit unit;

    private void Start()
    {
        unit = GetComponentInParent<Unit>();
    }
    
    private void Update()
    {
        SetHealth(unit.hitPoints, unit.maxHP);
    }
    
    public void SetHealth(float health, float maxHealth)
    {
        slider.value = health;
        slider.maxValue = maxHealth;
    }
}
