using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstantDeathEffect", menuName = "Effects/Instant Death Effect")]
public class InstantDeathEffect : Effect
{
    protected override void OnDeactivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HeroSM outHeroSM))
        {
            outHeroSM.die();
        }
    }
}
