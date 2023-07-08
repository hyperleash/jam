using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectPickup : MonoBehaviour
{
    [SerializeField]
    private float _durationScaling = 1f;
    [SerializeField]
    private Effect[] _effectInstances;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EffectBehaviour outEffectBehaviour)) 
        {
            foreach (Effect effectInstance in _effectInstances)
            {
                var effect = Instantiate(effectInstance).Initialize(outEffectBehaviour);
                effect.Duration *= _durationScaling;
            }

            Destroy(gameObject);
        }
    }
}