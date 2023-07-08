using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class Effect : ScriptableObject
{
    public float Duration => _duration;
    public EffectBehaviour EffectBehaviour => _behaviour;

    [SerializeField, Min(0)]
    private float _duration = 4;

    private EffectBehaviour _behaviour;
    private CancellationTokenSource _destroyTokenSource = new CancellationTokenSource();

    public void Initialize(EffectBehaviour behaviour)
    {
        _behaviour = behaviour;
        OnActivate(behaviour);
        Update().AttachExternalCancellation(_destroyTokenSource.Token);
    }

    protected virtual void OnActivate(EffectBehaviour behaviour) { }
    protected virtual void OnDeactivate(EffectBehaviour behaviour) { }

    private async UniTask Update()
    {
        await UniTask.WaitForSeconds(Duration);
        OnDeactivate(_behaviour);
        Destroy(this);
    }

    private void OnDestroy()
    {
        _destroyTokenSource.Cancel();
        _destroyTokenSource.Dispose();
    }
}
