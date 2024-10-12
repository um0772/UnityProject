using System.Collections;
using UnityEngine;

/// <summary>
/// This script defines 'Enemy's' health and behavior.
/// </summary>
public class Enemy : MonoBehaviour
{
    [Tooltip("Health points in integer")]
    public int health;
    public int enemyScore;
    [Tooltip("Enemy's projectile prefab")]
    public GameObject Projectile;
    public GameObject player;
    [Tooltip("VFX prefab generating after destruction")]
    public GameObject destructionVFX;
    public GameObject hitEffect;
    public GameObject Power4;

    [HideInInspector] public int shotChance; //probability of 'Enemy's' shooting during the path
    [HideInInspector] public float shotTimeMin, shotTimeMax; //max and min time for shooting from the beginning of the path

    private void Start()
    {
        StartCoroutine(ActivateShooting());
    }

    // Coroutine for enemy shooting
    IEnumerator ActivateShooting()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(shotTimeMin, shotTimeMax));
            if (Random.value < (float)shotChance / 100) // Check if enemy should shoot based on shotChance
            {
                Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
            }
        }
    }

    // Method for taking damage
    public void GetDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (Player.instance != null)
            {
                Player.instance.AddScore(enemyScore); // 적을 처치했을 때 점수 추가
                int ran = Random.Range(0, 10);
                if(ran < 9)
                {
                    Debug.Log("Not Item");

                }
                else if (ran < 10)
                {
                    Instantiate(Power4, transform.position, Power4.transform.rotation);
                }
            }
            Destruction();
        }
        else
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity, transform);
        }
    }

    // Trigger event when enemy collides with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if the collision has a Projectile component
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile != null)
            {
                Player.instance?.GetDamage(projectile.damage); // null 체크 추가
            }
            else
            {
                Player.instance?.GetDamage(1); // Default damage if Projectile component not found (null 체크 추가)
            }
        }
    }

    // Method for destroying the enemy
    void Destruction()
    {
        Instantiate(destructionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
