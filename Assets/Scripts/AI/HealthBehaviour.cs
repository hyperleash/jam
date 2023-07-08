using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public int Health
    {
        get => _health;
        set
        {
            value = Mathf.Max(value, 0);  // Cannot be negative.

            if (_health == value)  // Health is unaffected.
                return;

            OnHealthChangedCallback((value, value - _health));

            if (value <= 0)
                OnDeathCallback();

            _health = value;
        }
    }

    public event Action<(int health, int change)> OnHealthChangedCallback = delegate { };
    public event Action OnDeathCallback = delegate { };

    private int _health;
}
