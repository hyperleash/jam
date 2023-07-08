using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            value = Mathf.Max(value, 0);

            if (_maxHealth == value) 
                return;

            OnMaxHealthChangedCallback((value, value - _maxHealth));

            if (_maxHealth < _health)
                _health = value;

            if (value <= 0)
                OnDeathCallback();

            _maxHealth = value;
        }
    }

    public int Health
    {
        get => _health;
        set
        {
            value = Mathf.Clamp(value, 0, _maxHealth);  // Cannot be negative or greather han max health.

            if (_health == value)  // Health is unaffected.
                return;

            OnHealthChangedCallback((value, value - _health));

            if (value <= 0)
                OnDeathCallback();

            _health = value;
        }
    }

    public event Action<(int health, int change)> OnHealthChangedCallback = delegate { };
    public event Action<(int maxHealth, int change)> OnMaxHealthChangedCallback = delegate { };
    public event Action OnDeathCallback = delegate { };

    [SerializeField, Min(0)]
    private int _health = 10;
    [SerializeField, Min(0)]
    private int _maxHealth = 10;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
#endif
}
