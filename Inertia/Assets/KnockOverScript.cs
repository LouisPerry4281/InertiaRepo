using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOverScript : MonoBehaviour
{
    Transform player;
    Rigidbody rb;

    [SerializeField] float force;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13 || other.gameObject.layer == 7) //Player Weapon and Player Layers
        {
            rb.AddForce((player.transform.position - transform.position).normalized * force * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
