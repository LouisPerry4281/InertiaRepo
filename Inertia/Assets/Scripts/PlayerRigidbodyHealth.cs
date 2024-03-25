using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigidbodyHealth : MonoBehaviour
{
    PlayerRigidbodyMovement playerMovement;
    GameManager gameManager;

    [SerializeField] float maxHealth;
    public float currentHealth;

    [SerializeField] float invulnFrames;
    bool isVulnerable = true;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = GetComponent<PlayerRigidbodyMovement>();

        currentHealth = maxHealth;
    }

    public void InitializeDamage(float damageToTake)
    {
        if (!isVulnerable)
        {
            return;
        }

        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            gameManager.KillPlayer();
        }

        playerMovement.JuiceChange(-0.4f);

        isVulnerable = false;
        Invoke("ReVulnerable", invulnFrames);
    }

    void ReVulnerable()
    {
        isVulnerable = true;
    }
}
