using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "InvisibilityEffect", menuName = "Effects/Invisibility Effect")]
public class InvisibilityFrameEffect : Effect
{
    private CancellationTokenSource _cancellationTokenSource;
    
    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HealthBehaviour outHealthBehaviour))
        {
            _cancellationTokenSource = outHealthBehaviour.AddInvisibilityFramesWithCancellation(Duration);
        }
    }

    protected override void OnDeactivate(EffectBehaviour behaviour)
    {
        if (_cancellationTokenSource is null)
            return;
        Debug.Log("cancel!");
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
