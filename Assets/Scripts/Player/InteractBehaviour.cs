using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private LayerMask _placeableMask;

    private Collider2D _dragging;
    private Collider2D _ghostCollider;
    private Camera _camera;

    private Collider2D[] _colliding = new Collider2D[32];
    private RaycastHit2D[] _hits = new RaycastHit2D[32];

    public void MoveCursor(InputAction.CallbackContext context)
    {
        if (_dragging is null)  // If not dragging anything.
            return;

        if (context.performed)
            PlaceGameObject(ref _ghostCollider);
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

                    if (_dragging.TryGetComponent<SpriteRenderer>(out var outSpriteRenderer))
                    {
                        GameObject ghostObject = new GameObject("GhostObject");
                        ghostObject.layer = _dragging.gameObject.layer;
                        ghostObject.transform.position = _dragging.transform.position;
                        ghostObject.transform.rotation = _dragging.transform.rotation;
                        ghostObject.transform.localScale = _dragging.transform.localScale;

                        var ghostSprite = ghostObject.AddComponent<SpriteRenderer>();
                        ghostSprite.sprite = outSpriteRenderer.sprite;
                        ghostSprite.color = outSpriteRenderer.color.WithAlpha(outSpriteRenderer.color.a / 2);
                        ghostSprite.flipX = outSpriteRenderer.flipX;
                        ghostSprite.flipY = outSpriteRenderer.flipY;
                        ghostSprite.drawMode = outSpriteRenderer.drawMode;
                        ghostSprite.maskInteraction = outSpriteRenderer.maskInteraction;
                        ghostSprite.spriteSortPoint = outSpriteRenderer.spriteSortPoint;
                        ghostSprite.material = outSpriteRenderer.material;
                        ghostSprite.sortingLayerID = outSpriteRenderer.sortingLayerID;
                        ghostSprite.sortingOrder = outSpriteRenderer.sortingOrder;
                        ghostSprite.renderingLayerMask = outSpriteRenderer.renderingLayerMask;
                        
                        var ghostCollider = ghostObject.AddComponent<BoxCollider2D>();

                        if (_dragging is BoxCollider2D boxCollider)
                            ghostCollider.size = boxCollider.size;
                        else if (_dragging is CapsuleCollider2D capsuleCollider)
                            ghostCollider.size = capsuleCollider.size;
                        else
                            ghostCollider.size = _dragging.bounds.size;
    
                        ghostCollider.offset = _dragging.offset;
                        ghostCollider.isTrigger = true;
                        _ghostCollider = ghostCollider;
                    }

                    break;
                }
            }
        }
        else if (context.canceled)
        {
            Destroy(_ghostCollider.gameObject);
            PlaceGameObject(ref _dragging);

            _dragging.isTrigger = false;

            _ghostCollider = null;
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
}
