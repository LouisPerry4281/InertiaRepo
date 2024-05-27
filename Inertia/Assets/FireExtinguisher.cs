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

    [SerializeField] float initialForce;
    [SerializeField] float force;
    [SerializeField] float forceMultiplier;

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
            rb.AddForce((player.transform.position - transform.position).normalized * initialForce * Time.deltaTime, ForceMode.Impulse);
            AudioManager.instance.PlaySFX("MetalHit", 0.5f, 1);
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
        rb.AddForce(transform.up * force * Time.deltaTime);
    }


}
