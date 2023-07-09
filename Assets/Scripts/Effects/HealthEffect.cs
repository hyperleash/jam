using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthEffect", menuName = "Effects/Health Effect")]
public class HealthEffect : Effect
{
    [SerializeField]
    private int _healthChange = 20;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HealthBehaviour outHealthBehaviour))
        {
            outHealthBehaviour.Health += _healthChange;
        }
    }
}
