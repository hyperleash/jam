using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpEffect", menuName = "Effects/Jump Effect")]
public class JumpEffect : Effect
{
    [SerializeField]
    private float _jumpPercentage = 1.4f;

    private float _previousJumpForce;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HeroSM outHeroSM))
        {
            _previousJumpForce = outHeroSM.jumpForce;
            outHeroSM.jumpForce *= _jumpPercentage;
        }
    }

    protected override void OnDeactivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HeroSM outHeroSM))
        {
            outHeroSM.jumpForce = _previousJumpForce;
        }
    }
}
