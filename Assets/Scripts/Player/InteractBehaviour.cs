using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private LayerMask _placeableMask;

    private Collider2D _dragging;
    private Camera _camera;

    private Collider2D[] _colliding = new Collider2D[32];
    private RaycastHit2D[] _hits = new RaycastHit2D[32];

    public void DragAndDrop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var hits = Physics2D.GetRayIntersectionAll(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));

            if (hits.Length <= 0)
                return;

            foreach (var hit in hits)
            {
                var collider = hit.collider;

                if (((1 << collider.gameObject.layer) & _interactableMask) != 0)
                {
                    _dragging = collider;
                    break;
                }
            }
        }
        else if (context.canceled)
        {
            if (_dragging is null)
                return;

            Vector2 currentPoint = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).xy_();

            while (true)
            {
                int colliderCount = Physics2D.OverlapBoxNonAlloc(new Vector2(currentPoint.x, currentPoint.y + 0.05f), _dragging.bounds.size,
                    _dragging.transform.rotation.z, _colliding, _placeableMask);

                if (colliderCount <= 0) // If there is no overlap issues.
                    break;

                currentPoint.y = _colliding.First().bounds.max.y + _dragging.bounds.size.y / 2;
                _dragging.transform.position = currentPoint;
            }

            int hitCount = Physics2D.BoxCastNonAlloc(
                currentPoint, _dragging.bounds.size.xy(), _dragging.transform.rotation.z, 
                Vector2.down, _hits, float.MaxValue, _placeableMask);

            if (hitCount <= 0)
                return;

            currentPoint.y = _hits.First().point.y + _dragging.bounds.size.y / 2;
            _dragging.transform.position = currentPoint;

            _dragging = null;
        }
    }

    private void Awake()
    {
        _camera = Camera.main;
    }
}
