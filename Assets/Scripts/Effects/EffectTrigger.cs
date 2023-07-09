using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public IEnumerable<Effect> EffectInstances => _effectInstances;

    [SerializeField] private string _id;
    [SerializeField] private float _durationScaling = 1f;
    [SerializeField] private bool _triggeredOnce = true;
    [SerializeField] private Effect[] _effectInstances;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ApplyEffectsTo(collision.gameObject))
        {
            if (_triggeredOnce)
                Destroy(gameObject);
        }
    }

    public bool ApplyEffectsTo(GameObject toAffect)
    {
        if (toAffect.TryGetComponent(out EffectBehaviour outEffectBehaviour))
        {
            foreach (Effect effectInstance in _effectInstances)
            {
                Effect effect = Instantiate(effectInstance);
                effect.Duration *= _durationScaling;
                effect.Id = _id;
                outEffectBehaviour.AddEffect(effect);
                effect.Initialize(outEffectBehaviour);
            }

            return true;
        }

        return false;
    }
}
