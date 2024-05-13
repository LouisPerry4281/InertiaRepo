using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigidbodyHealth : MonoBehaviour
{
    PlayerRigidbodyMovement playerMovement;
    PlayerCombatCombo playerCombat;
    GameManager gameManager;
    Animator anim;

    [SerializeField] float maxHealth;
    public float currentHealth;

    [SerializeField] float invulnFrames;
    bool isVulnerable = true;

    [SerializeField] float juiceLossOnHit = 0.2f;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = GetComponent<PlayerRigidbodyMovement>();
        playerCombat = GetComponent<PlayerCombatCombo>();
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
    }

    public void InitializeDamage(float damageToTake)
    {
        if (!isVulnerable) //If the player has iFrames, cancel damage
        {
            return;
        }

        currentHealth -= damageToTake;

        if (currentHealth <= 0)
        {
            gameManager.KillPlayer();
            return;
        }

        playerCombat.EndCombo();
        anim.Play("Hurt");

        playerMovement.JuiceChange(-juiceLossOnHit); //When hit, reduce the player's juice

        isVulnerable = false;
        Invoke("ReVulnerable", invulnFrames);
    }

    void ReVulnerable()
    {
        isVulnerable = true;
    }
}
