using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Flash flash;
    [SerializeField] private float invincibilityDuration = 1f;
    [SerializeField] private UnityEngine.UI.Image faceImg;
    [SerializeField] private Sprite normalFace;
    [SerializeField] private Sprite hurtFace;
    [SerializeField] private float faceDuration = 0.5f;
    [SerializeField] private PlayerShoot playerShoot;
    private bool isInvincible;
    public int health;
    public int maxHealth = 3;
    Vector2 startPos;
    private Coroutine faceRoutine;
    public SpriteRenderer playerSr;
    public PlayerMovement playerMovement;
    [SerializeField] public float respawnTime = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        startPos = transform.position;
    }

    public void TakeDamage(int amount)
    {
        if (isInvincible)
        {
            return;
        }
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        flash.SimpleFlash();
        if (faceRoutine != null)
        {
            StopCoroutine(faceRoutine);
        }

        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        faceRoutine = StartCoroutine(HurtFaceRoutine());
        StartCoroutine(InvincibleRoutine());
        if(health <= 0)
        {
            playerSr.enabled = false;
            playerMovement.enabled = false;
            Die();
        }
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private IEnumerator HurtFaceRoutine()
    {
        faceImg.sprite = hurtFace;
        faceImg.color = Color.magenta;

        yield return new WaitForSeconds(faceDuration);

        faceImg.sprite = normalFace;
        faceImg.color = Color.white;
        faceRoutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);

        Respawn();
    }

    void Respawn()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        transform.position = startPos;
        health = maxHealth;

        isInvincible = false;

        playerSr.enabled = true;
        playerMovement.enabled = true;
        playerShoot.ResetAmmo();
        StartCoroutine(InvincibleRoutine());
    }
}
