using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class Effect : ScriptableObject
{
    public float Duration
    {
        get => _duration;
        set => _duration = Mathf.Max(value, 0);
    }

    public string Id
    {
        get => _id;
        set => _id = value;
    }

    public EffectBehaviour EffectBehaviour => _behaviour;

    public event Action<Effect> OnDeactivateCallback = delegate { };

    [SerializeField, Min(0)]
    private float _duration = 4;

    private string _id;

    private EffectBehaviour _behaviour;
    private CancellationTokenSource _destroyTokenSource = new CancellationTokenSource();

    public Effect Initialize(EffectBehaviour behaviour)
    {
        _behaviour = behaviour;
        OnActivate(behaviour);
        Wait(_destroyTokenSource.Token).Forget();

        return this;
    }

    protected virtual void OnActivate(EffectBehaviour behaviour) { }
    protected virtual void OnDeactivate(EffectBehaviour behaviour) { }

    private async UniTask Wait(CancellationToken cancellationToken)
    {
        float duration = float.IsInfinity(Duration) ? 1_000_000 : Duration;
        float startTime = Time.time;

        while ((duration + startTime > Time.time) && !cancellationToken.IsCancellationRequested)
            await UniTask.WaitForFixedUpdate();

        if (!cancellationToken.IsCancellationRequested)
            Destroy(this);
    }

    private void OnDestroy()
    {
        if (_behaviour != null)
        {
            OnDeactivate(_behaviour);
            OnDeactivateCallback(this);
        }

        _destroyTokenSource.Cancel();
        _destroyTokenSource.Dispose();
    }
}
