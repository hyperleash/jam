using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEffect", menuName = "Effects/Health Effect")]
public class HealthEffect : Effect
{
    [SerializeField]
    private float _healthChange = 20;

    private float _previousGravity;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HealthBehaviour outHealthBehaviour))
        {
            outHealthBehaviour.Health += 20;
        }
    }
}
