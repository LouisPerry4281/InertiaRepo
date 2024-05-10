using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorController : MonoBehaviour
{
    SpawnDoorController spawnController;

    GameObject gM;

    private void Start()
    {
        spawnController = GetComponent<SpawnDoorController>();
        spawnController.enabled = false;

        gM = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //Checks for player layer
        {
            GetComponentInChildren<Animator>().Play("DoorOpen");
            gM.GetComponent<AudioManager>().PlaySFX("Door", 1, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7) //Checks for player layer
        {
            GetComponentInChildren<Animator>().Play("DoorClose");
            gM.GetComponent<AudioManager>().PlaySFX("Door", 1, 1);
            spawnController.enabled = true;

            GameObject.Find("SequenceController").GetComponent<SequenceManager>().StartCombat();

            //GetComponent<BoxCollider>().enabled = false;
            Destroy(this);
        }
    }
}
