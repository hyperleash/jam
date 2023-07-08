using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour
{
    public IReadOnlyList<Effect> ActiveEffects => _activeEffects;

    [SerializeField, ReadOnly]
    private List<Effect> _activeEffects;

    public Effect AddEffect(Effect effect)
    {
        effect.OnDeactivateCallback += deactiveEffect => _activeEffects.Remove(deactiveEffect); 
        _activeEffects.Add(effect);

        return effect;
    }

    public void RemoveEffect(Effect effect)
    {
        Destroy(effect);
    }

    public void RemoveEffectWithId(string id)
    {
        var toRemove = _activeEffects.Where(x => x.Id == id).ToList();

        for (int i = toRemove.Count; i >= 0; i--)
            Destroy(toRemove[i]);
    }

    private void Awake()
    {
        _activeEffects = new List<Effect>();
    }
}
