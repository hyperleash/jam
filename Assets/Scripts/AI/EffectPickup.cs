using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EffectPickup : MonoBehaviour
{
    [SerializeField]
    private Effect _effectInstance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EffectBehaviour outEffectBehaviour)) 
        {
            Instantiate(_effectInstance).Initialize(outEffectBehaviour);
            Destroy(gameObject);
        }
    }
}
