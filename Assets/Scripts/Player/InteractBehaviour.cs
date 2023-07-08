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
    private GameObject _ghostObject;
    private Camera _camera;

    private Collider2D[] _colliding = new Collider2D[32];
    private RaycastHit2D[] _hits = new RaycastHit2D[32];

    public void MoveCursor(InputAction.CallbackContext context)
    {
        if (_dragging is null)  // If not dragging anything.
            return;

        if (context.performed)
            PlaceGameObject(ref _dragging);
    }

    public void DragAndDrop(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            int hitCount = Physics2D.GetRayIntersectionNonAlloc(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()), _hits);

            if (hitCount <= 0)
                return;

            for (int i = 0; i < hitCount; i++)
            {
                var collider = _hits[i].collider;

                if (((1 << collider.gameObject.layer) & _interactableMask) != 0)
                {
                    _dragging = collider;
                    _dragging.isTrigger = true;

                    if (_dragging.TryGetComponent<SpriteRenderer>(out var outSpriteRenderer) && _dragging.TryGetComponent<Collider2D>(out var outCollider2D))
                    {
                        _ghostObject = new GameObject("GhostObject");
                        _ghostObject.layer = _dragging.gameObject.layer;
                        _ghostObject.transform.position = _dragging.transform.position;
                        _ghostObject.transform.rotation = _dragging.transform.rotation;
                        _ghostObject.transform.localScale = _dragging.transform.localScale;
                        CopySpriteRenderer(_ghostObject.AddComponent<SpriteRenderer>(), outSpriteRenderer);
                    }

                    break;
                }
            }
        }
        else if (context.canceled)
        {
            Destroy(_ghostObject);
            PlaceGameObject(ref _dragging);

            _dragging.isTrigger = false;

            _ghostObject = null;
            _dragging = null;
        }
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void PlaceGameObject(ref Collider2D collider)
    {
        if (collider is null)
            return;

        Vector2 currentPoint = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).xy_();

        Debug.Log(collider.gameObject);
        while (true)
        {
            int colliderCount = Physics2D.OverlapBoxNonAlloc(new Vector2(currentPoint.x, currentPoint.y + 0.05f), collider.bounds.size,
                collider.transform.rotation.z, _colliding, _placeableMask);

            if (colliderCount <= 0) // If there is no overlap issues.
                break;

            currentPoint.y = _colliding.First().bounds.max.y + collider.bounds.size.y / 2;
            collider.transform.position = currentPoint;
        }

        int hitCount = Physics2D.BoxCastNonAlloc(
            currentPoint, collider.bounds.size.xy(), collider.transform.rotation.z,
            Vector2.down, _hits, float.MaxValue, _placeableMask);

        if (hitCount <= 0)
            return;

        currentPoint.y = _hits.First().point.y + collider.bounds.size.y / 2;
        collider.transform.position = currentPoint;
    }

    private void CopySpriteRenderer(SpriteRenderer copy, SpriteRenderer original)
    {
        copy.sprite = original.sprite;
        copy.color = original.color;
        copy.flipX = original.flipX;
        copy.flipY = original.flipY;
        copy.drawMode = original.drawMode;
        copy.maskInteraction = original.maskInteraction;
        copy.spriteSortPoint = original.spriteSortPoint;
        copy.material = original.material;
        copy.sortingLayerID = original.sortingLayerID;
        copy.sortingOrder = original.sortingOrder;
        copy.renderingLayerMask = original.renderingLayerMask;
    }
}
