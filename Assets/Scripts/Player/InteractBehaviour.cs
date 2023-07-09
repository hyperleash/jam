using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class InteractBehaviour : MonoBehaviour
{
    public bool IsPlaceableZoneRequired
    {
        get => _isPlaceableZoneRequired;
        set => _isPlaceableZoneRequired = value;
    }

    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private LayerMask _placeableMask;
    [SerializeField] private LayerMask _placeableZoneMask;
    [SerializeField] private float _tileSizeSnapping = 1.0f;
    [SerializeField] private bool _isPlaceableZoneRequired = true;

    private Collider2D _dragging;
    private Collider2D _ghostCollider;
    private SpriteRenderer _ghostSprite;
    private Camera _camera;

    private Collider2D[] _colliding = new Collider2D[32];
    private RaycastHit2D[] _hits = new RaycastHit2D[32];

    public void MoveCursor(InputAction.CallbackContext context)
    {
        if (_dragging == null)  // If not dragging anything.
            return;

        if (context.performed)
        {
            if (PlaceGameObject(ref _ghostCollider))
                _ghostSprite.enabled = true; 
            else
                _ghostSprite.enabled = false; // Hide if there is no valid placeable location
        }
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

                    if (_dragging.TryGetComponent<SpriteRenderer>(out var outSpriteRenderer))
                    {
                        GameObject ghostObject = new GameObject("GhostObject");
                        ghostObject.layer = _dragging.gameObject.layer;
                        ghostObject.transform.position = _dragging.transform.position;
                        ghostObject.transform.rotation = _dragging.transform.rotation;
                        ghostObject.transform.localScale = _dragging.transform.localScale;

                        var ghostSprite = ghostObject.AddComponent<SpriteRenderer>();
                        ghostSprite.sprite = outSpriteRenderer.sprite;
                        Color color = outSpriteRenderer.color;
                        color.a /= 2;
                        ghostSprite.color = color;
                        ghostSprite.flipX = outSpriteRenderer.flipX;
                        ghostSprite.flipY = outSpriteRenderer.flipY;
                        ghostSprite.drawMode = outSpriteRenderer.drawMode;
                        ghostSprite.maskInteraction = outSpriteRenderer.maskInteraction;
                        ghostSprite.spriteSortPoint = outSpriteRenderer.spriteSortPoint;
                        ghostSprite.material = outSpriteRenderer.material;
                        ghostSprite.sortingLayerID = outSpriteRenderer.sortingLayerID;
                        ghostSprite.sortingOrder = outSpriteRenderer.sortingOrder;
                        ghostSprite.renderingLayerMask = outSpriteRenderer.renderingLayerMask;
                        _ghostSprite = ghostSprite;

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
            if (_ghostCollider != null)
            {
                Destroy(_ghostCollider.gameObject);
                _ghostCollider = null;
                _ghostSprite = null;
            }

            if (_dragging != null)
            {
                PlaceGameObject(ref _dragging);
                _dragging = null;
            }
        }
    }

    private void Awake()
    {
        _camera = Camera.main;
    }

    private bool PlaceGameObject(ref Collider2D collider)
    {
        if (collider == null)
            return false;

        Vector2 cursorPoint = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()).xy_();
        Vector2 currentPoint = new Vector2(
            Mathf.Round(cursorPoint.x / _tileSizeSnapping) * _tileSizeSnapping, cursorPoint.y);

        while (true)
        {
            int colliderCount = Physics2D.OverlapBoxNonAlloc(new Vector2(currentPoint.x, currentPoint.y + 0.05f), collider.bounds.size,
                collider.transform.rotation.z, _colliding, _placeableMask);

            if (colliderCount <= 0) // If there is no overlap issues.
                break;

            currentPoint.y = _colliding.First().bounds.max.y + collider.bounds.size.y / 2;
        }

        int hitCount = Physics2D.BoxCastNonAlloc(
            currentPoint, collider.bounds.size.xy(), collider.transform.rotation.z,
            Vector2.down, _hits, float.MaxValue, _placeableMask);

        if (hitCount <= 0)
            return false;

        currentPoint.y = _hits.First().point.y + collider.bounds.size.y / 2;

        if (_isPlaceableZoneRequired)
        {
            Bounds bounds = collider.bounds;
            Vector2 topRight = new Vector2(currentPoint.x + bounds.extents.x, currentPoint.y + bounds.extents.y);
            Vector2 topLeft = new Vector2(currentPoint.x - bounds.extents.x, currentPoint.y + bounds.extents.y);
            Vector2 bottomRight = new Vector2(currentPoint.x + bounds.extents.x, currentPoint.y - bounds.extents.y);
            Vector2 bottomLeft = new Vector2(currentPoint.x - bounds.extents.x, currentPoint.y - bounds.extents.y);

            bool OverlapPoint(Vector2 point)
            {
                int colliderCount = Physics2D.OverlapPointNonAlloc(point, _colliding, _placeableZoneMask);

                if (colliderCount <= 0)
                    return false;

                return true;
            }

            // All corners need to be inside of a placeable zone.
            if (!(OverlapPoint(topRight) && OverlapPoint(topLeft) && OverlapPoint(bottomRight) && OverlapPoint(bottomLeft)))
                return false;
        }

        {
            int colliderCount = Physics2D.OverlapBoxNonAlloc(new Vector2(currentPoint.x, currentPoint.y), collider.bounds.size,
                   collider.transform.rotation.z, _colliding, _interactableMask);

            if (colliderCount > 0)
            {
                for (int i = 0; i < colliderCount; i++)
                {
                    if (_colliding[i] != _dragging && _colliding[i] != _ghostCollider)  // Pervent placing on each other.
                        return false;
                }
            }
        }

        collider.transform.position = currentPoint;

        return true;
    }
}
