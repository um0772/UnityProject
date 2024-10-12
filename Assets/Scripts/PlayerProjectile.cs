using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the damage and behavior for the player's projectiles.
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    [Tooltip("Damage which the projectile deals to another object. Integer")]
    public int damage;

    [Tooltip("Whether the projectile is destroyed in the collision, or not")]
    public bool destroyedByCollision;

    private void OnTriggerEnter2D(Collider2D collision) // when a projectile collides with another object
    {
        if (collision.CompareTag("Enemy")) // if the projectile belongs to the player and hits an enemy
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamage(damage);
            }
            if (destroyedByCollision)
            {
                Destruction();
            }
        }
    }

    void Destruction()
    {
        Destroy(gameObject);
    }
}
