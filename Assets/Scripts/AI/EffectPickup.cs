using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectPickup : MonoBehaviour
{
    [SerializeField]
    private Effect[] _effectInstances;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EffectBehaviour outEffectBehaviour)) 
        {
            foreach (Effect effectInstance in _effectInstances)
            {
                Instantiate(effectInstance).Initialize(outEffectBehaviour);
            }

            Destroy(gameObject);
        }
    }
}
