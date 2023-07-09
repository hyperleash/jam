using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CancelOnHitEffect", menuName = "Effects/Cancel On Hit Effect")]
public class CancelOnHitEffect : Effect
{
    [SerializeField]
    private string _idToCancel;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out HealthBehaviour outHealthBehaviour))
        {
            outHealthBehaviour.OnHealthChangingCallback += OnHealthChaning;
        }

        void OnHealthChaning((int health, int change) value)
        {
            if (value.change < 0) // If the player is taking damage.
            {
                if (behaviour.TryGetComponent(out EffectBehaviour outEffectBehaviour))
                {
                    outEffectBehaviour.RemoveEffectWithId(_idToCancel);
                    outHealthBehaviour.OnHealthChangingCallback -= OnHealthChaning;

                    Destroy(this);
                }
            }
        }
    }

}
