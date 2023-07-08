using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public int Health
    {
        get => _health;
        set
        {
            value = Mathf.Clamp(value, 0, _maxHealth);  // Cannot be negative or greather han max health.

            if (_health == value)  // Health is unaffected.
                return;

            OnHealthChangedCallback((value, value - _health));
            _onHealthChanged.Invoke(value);

            if (value <= 0)
            {
                OnDeathCallback();
                _onDeath.Invoke();
            }

            _health = value;
        }
    }

    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            value = Mathf.Max(value, 0);

            if (_maxHealth == value)
                return;

            OnMaxHealthChangedCallback((value, value - _maxHealth));
            _onMaxHealthChanged.Invoke(value);

            if (_maxHealth < _health)
                _health = value;

            if (value <= 0)
            {
                OnDeathCallback();
                _onDeath.Invoke();
            }

            _maxHealth = value;
        }
    }

    public event Action<(int health, int change)> OnHealthChangedCallback = delegate { };
    public event Action<(int maxHealth, int change)> OnMaxHealthChangedCallback = delegate { };
    public event Action OnDeathCallback = delegate { };

    [SerializeField, Min(0)]
    private int _health = 100;
    [SerializeField, Min(0)]
    private int _maxHealth = 100;

    [SerializeField]
    private UnityEvent<int> _onHealthChanged;
    [SerializeField]
    private UnityEvent<int> _onMaxHealthChanged;
    [SerializeField]
    private UnityEvent _onDeath;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health > _maxHealth)
            _health = _maxHealth;
    }
#endif
}
