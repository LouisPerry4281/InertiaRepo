using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    Transform player;
    Rigidbody rb;

    bool isDamaged;

    [SerializeField] GameObject smokeVFX;
    [SerializeField] Transform smokeTarget;
    [SerializeField] GameObject motif;

    [SerializeField] float initialForce;
    [SerializeField] float force;
    [SerializeField] float forceMultiplier;
    [SerializeField] float maxForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 && !isDamaged) //Player Weapon Layer
        {
            isDamaged = true;
            rb.isKinematic = false;
            rb.AddForceAtPosition((player.transform.position - transform.position).normalized * initialForce * Time.deltaTime, player.position, ForceMode.Impulse);
            Instantiate(smokeVFX, smokeTarget);

            rb.excludeLayers = 7;

            AudioManager.instance.PlaySFX("MetalHit", 0.5f, 1);
            AudioManager.instance.PlaySFX("Hiss", 0.7f, 1.5f);

            Invoke("Explode", 5);
        }
    }

    private void Update()
    {
        if (isDamaged)
        {
            force *= forceMultiplier;
            Invoke("Launch", 0.1f);
        }
    }

    void Launch()
    {
        rb.AddForce(-transform.up * Mathf.Clamp(force, 0.0f, maxForce) * Time.deltaTime);
    }

    void Explode()
    {
        AudioManager.instance.PlaySFX("Pop", 1.5f, 0.5f);
        Instantiate(motif, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
