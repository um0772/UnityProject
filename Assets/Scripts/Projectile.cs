using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the damage and defines whether the projectile belongs to the ‘Enemy’ or to the ‘Player’, whether the projectile is destroyed in the collision, or not and amount of damage.
/// </summary>

public class Projectile : MonoBehaviour
{

    [Tooltip("Damage which a projectile deals to another object. Integer")]
    public int damage;

    [Tooltip("Whether the projectile belongs to the ‘Enemy’ or to the ‘Player’")]
    public bool enemyBullet;

    [Tooltip("Whether the projectile is destroyed in the collision, or not")]
    public bool destroyedByCollision;

    private void OnTriggerEnter2D(Collider2D collision) //when a projectile collides with another object
    {
        if (enemyBullet && collision.CompareTag("Player")) // if the projectile belongs to the enemy and hits the player
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.GetDamage(damage);
            }
            if (destroyedByCollision)
            {
                Destruction();
            }
        }
        else if (!enemyBullet && collision.CompareTag("Enemy")) // if the projectile belongs to the player and hits an enemy
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
