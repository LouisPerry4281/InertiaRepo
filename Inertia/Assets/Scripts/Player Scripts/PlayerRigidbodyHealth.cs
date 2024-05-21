using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    public Vignette vignetteComponent;

    [SerializeField] VolumeProfile vp;

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

    private void Update()
    {

        //GameObject.Find("Global Volume").GetComponent<Volume>().profile.TryGet<Vignette>(out vignetteComponent);
        //vignetteComponent.intensity = new ClampedFloatParameter(0.2f, 0, 1, true);

        //vp.TryGet<Vignette>(out vignetteComponent);
        //vignetteComponent.intensity = new ClampedFloatParameter(0.2f, 0, 1, true);
    }




}
