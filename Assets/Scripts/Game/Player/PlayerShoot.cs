using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunOffset;
    [SerializeField] private float bulletDelay;
    private float lastFiredTime;
    private bool fireContinuously;
    //private bool fireSingle;

    //all variables under here are related to reloading + ammo amount
    public int currentAmmo = 0;
    public int maxAmmo = 99;
    [SerializeField] public float ammoRegen = 3f;
    [SerializeField] public float proximityRegen = 0.7f;
    private bool inProximity = false;
    private float timer;
    [SerializeField] private TextMeshProUGUI ammoText;

    // Update is called once per frame
    void Update()
    {
        ammoText.text = "HeartBeats: " + currentAmmo;
        ammoText.color = inProximity ? Color.magenta : Color.white;
        
        if (currentAmmo < maxAmmo)
        {
            timer += Time.deltaTime;

            float currentRegen = inProximity ? proximityRegen : ammoRegen;

            while (timer >= currentRegen)
            {
                timer -= currentRegen;
                currentAmmo++;
            }
        }
        else
        {
            timer = 0f;
        }
        if (fireContinuously && currentAmmo > 0)
        {
            float timeSinceLastFire = Time.time - lastFiredTime;

            if (timeSinceLastFire >= bulletDelay)
            {
                FireBullet();
                currentAmmo--;

                lastFiredTime = Time.time;
            }
            
        }
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunOffset.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.linearVelocity = bulletSpeed * transform.up;
    }
    private void OnFire(InputValue inputValue)
    {
        fireContinuously = inputValue.isPressed;
    }
/*
    public void EnterProximity()
    {
        nearbyEnemies++;
        Debug.Log("Nearby enemies: " + nearbyEnemies);
    }

    public void ExitProximity()
    {
        nearbyEnemies = Mathf.Max(0, nearbyEnemies - 1);
    }
    */

    public void SetProximityState(bool state)
    {
        inProximity = state;
    }

    public void ResetAmmo()
    {
        currentAmmo = 0;
        timer = 0f;
    }
}
