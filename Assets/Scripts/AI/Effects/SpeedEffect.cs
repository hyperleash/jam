using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedEffect", menuName = "Effects/Speed Effect")]
public class SpeedEffect : Effect
{
    [SerializeField]
    private float _speedPercentage = 1.4f;

    private float _previousSpeed;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HeroSM outHeroSM))
        {
            _previousSpeed = outHeroSM.speed;
            outHeroSM.speed *= _speedPercentage;
        }
    }

    protected override void OnDeactivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HeroSM outHeroSM))
        {
            outHeroSM.speed = _previousSpeed;
        }
    }
}
