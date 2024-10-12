using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Guns objects in 'Player's' hierarchy
[System.Serializable]
public class Guns
{
    public GameObject rightGun, leftGun, centralGun;
    [HideInInspector] public ParticleSystem leftGunVFX, rightGunVFX, centralGunVFX;
}

public class PlayerShooting : MonoBehaviour
{
    public Player player;
    [Tooltip("Shooting frequency. The higher the more frequent")]
    public float fireRate;

    [Tooltip("Projectile prefab")]
    public GameObject projectileObject;

    // Time for a new shot
    [HideInInspector] public float nextFire;

    [Tooltip("Current weapon power")]
    [Range(1, 4)]       // Change it if you wish
    public int weaponPower = 1;

    public Guns guns;
    [HideInInspector] public int maxWeaponPower = 4;

    // Flag to control shooting
    private bool shootingIsActive = false;

    public static PlayerShooting instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        // Receiving shooting visual effects components
        guns.leftGunVFX = guns.leftGun.GetComponent<ParticleSystem>();
        guns.rightGunVFX = guns.rightGun.GetComponent<ParticleSystem>();
        guns.centralGunVFX = guns.centralGun.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Check if spacebar is pressed to shoot
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootingIsActive = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseBoomItem();
        }

        if (shootingIsActive)
        {
            if (Time.time > nextFire)
            {
                MakeAShot();
                nextFire = Time.time + 1 / fireRate;
            }
        }
    }

    // Method for a shot
    void MakeAShot()
    {
        switch (weaponPower)
        {
            case 1:
                CreateLaserShot(projectileObject, guns.centralGun.transform.position, Vector3.zero);
                guns.centralGunVFX.Play();
                break;
            case 2:
                CreateLaserShot(projectileObject, guns.rightGun.transform.position, Vector3.zero);
                guns.leftGunVFX.Play();
                CreateLaserShot(projectileObject, guns.leftGun.transform.position, Vector3.zero);
                guns.rightGunVFX.Play();
                break;
            case 3:
                CreateLaserShot(projectileObject, guns.centralGun.transform.position, Vector3.zero);
                CreateLaserShot(projectileObject, guns.rightGun.transform.position, new Vector3(0, 0, -5));
                guns.leftGunVFX.Play();
                CreateLaserShot(projectileObject, guns.leftGun.transform.position, new Vector3(0, 0, 5));
                guns.rightGunVFX.Play();
                break;
            case 4:
                CreateLaserShot(projectileObject, guns.centralGun.transform.position, Vector3.zero);
                CreateLaserShot(projectileObject, guns.rightGun.transform.position, new Vector3(0, 0, -5));
                guns.leftGunVFX.Play();
                CreateLaserShot(projectileObject, guns.leftGun.transform.position, new Vector3(0, 0, 5));
                guns.rightGunVFX.Play();
                CreateLaserShot(projectileObject, guns.leftGun.transform.position, new Vector3(0, 0, 15));
                CreateLaserShot(projectileObject, guns.rightGun.transform.position, new Vector3(0, 0, -15));
                break;
        }

        // After shooting, deactivate shooting flag
        shootingIsActive = false;
    }

    void CreateLaserShot(GameObject laser, Vector3 pos, Vector3 rot)
    {
        Instantiate(laser, pos, Quaternion.Euler(rot));
    }

    void UseBoomItem()
    {
        // Check if boom item is available
        if (player != null && player.boom > 0 && !player.isBoomTime)
        {
            player.Boom();
        }
    }
}
