using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

            OnHealthChangingCallback((value, value - _health));
            _onHealthChanging.Invoke(value);

            if (value < _health)
            {
                if (_invisibilityFrameCount > 0) // In invisibility frame and cannot lose life.
                    return;

                AddInvisibilityFrames(); // Add invisibility if losing life.
            }

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

    public event Action<(int health, int change)> OnHealthChangingCallback = delegate { };
    public event Action<(int health, int change)> OnHealthChangedCallback = delegate { };
    public event Action<(int maxHealth, int change)> OnMaxHealthChangedCallback = delegate { };
    public event Action OnDeathCallback = delegate { };

    [SerializeField, Min(0)]
    private int _health = 100;
    [SerializeField, Min(0)]
    private int _maxHealth = 100;
    [SerializeField, Min(0)]
    private float _invisibilityDuration = 1;

    [SerializeField]
    private UnityEvent<int> _onHealthChanging;
    [SerializeField]
    private UnityEvent<int> _onHealthChanged;
    [SerializeField]
    private UnityEvent<int> _onMaxHealthChanged;
    [SerializeField]
    private UnityEvent _onDeath;

    private int _invisibilityFrameCount;

    public void AddInvisibilityFrames() => AddInvisibilityFrames(_invisibilityDuration);
    public void AddInvisibilityFrames(float duration) => InvisibilityFrame(duration, default).Forget();
    public CancellationTokenSource AddInvisibilityFramesWithCancellation(float duration)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        InvisibilityFrame(duration, cancellationTokenSource.Token).Forget();
        return cancellationTokenSource;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_health > _maxHealth)                                                                            
            _health = _maxHealth;
    }
#endif

    private async UniTaskVoid InvisibilityFrame(float duration, CancellationToken cancellationToken)
    {
        _invisibilityFrameCount++;

        duration = float.IsInfinity(duration) ? 1_000_000 : duration;
        float startTime = Time.time;

        while ((duration + startTime > Time.time) && !cancellationToken.IsCancellationRequested)
            await UniTask.WaitForFixedUpdate();

         _invisibilityFrameCount--;
    }
}
