using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GravityEffect", menuName = "Effects/Gravity Effect")]
public class GravityEffect : Effect
{
    [SerializeField]
    private float _gravity = 1;

    private float _previousGravity;

    protected override void OnActivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out Rigidbody2D outRigidBody2D))
        {
            _previousGravity = outRigidBody2D.gravityScale;
            outRigidBody2D.gravityScale = _gravity;
        }
    }

    protected override void OnDeactivate(EffectBehaviour behaviour)
    {
        if (behaviour.TryGetComponent(out Rigidbody2D outRigidBody2D))
        {
            outRigidBody2D.gravityScale = _previousGravity;
        }
    }
}
