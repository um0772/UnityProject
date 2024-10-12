using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script defines the size of the ‘Boundary’ depending on Viewport. When objects go beyond the ‘Boundary’, they are destroyed or deactivated.
/// </summary>
public class Boundary : MonoBehaviour
{
    BoxCollider2D boundaryCollider;

    private void Start()
    {
        boundaryCollider = GetComponent<BoxCollider2D>();
        ResizeCollider();
    }

    void ResizeCollider()
    {
        Vector2 viewportSize = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)) * 2;
        viewportSize.x *= 1.5f;
        viewportSize.y *= 1.5f;
        boundaryCollider.size = viewportSize;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") || collision.CompareTag("Bonus"))
        {
            Destroy(collision.gameObject);
        }
    }
}
