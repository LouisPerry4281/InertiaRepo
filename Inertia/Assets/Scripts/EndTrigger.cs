using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            endUI.SetActive(true);
            player.GetComponent<PlayerRigidbodyMovement>().enabled = false;
            player.GetComponent<PlayerRigidbodyHealth>().enabled = false;
            player.GetComponent<PlayerRigidbodyCombat>().enabled = false;
        }
    }
}
