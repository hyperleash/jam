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
        set => _duration = value;
    }

    public string Id
    {
        get => _message;
        set => _message = value;
    }

    public EffectBehaviour EffectBehaviour => _behaviour;

    public event Action<Effect> OnDeactivateCallback = delegate { };

    [SerializeField, Min(0)]
    private float _duration = 4;

    private string _message;

    private EffectBehaviour _behaviour;
    private CancellationTokenSource _destroyTokenSource = new CancellationTokenSource();

    public Effect Initialize(EffectBehaviour behaviour)
    {
        _behaviour = behaviour;
        OnActivate(behaviour);
        Update().AttachExternalCancellation(_destroyTokenSource.Token);

        return this;
    }

    protected virtual void OnActivate(EffectBehaviour behaviour) { }
    protected virtual void OnDeactivate(EffectBehaviour behaviour) { }

    private async UniTask Update()
    {
        await UniTask.WaitForSeconds(Duration);
        Destroy(this);
    }

    private void OnDestroy()
    {
        OnDeactivate(_behaviour);
        OnDeactivateCallback(this);

        _destroyTokenSource.Cancel();
        _destroyTokenSource.Dispose();
    }
}
