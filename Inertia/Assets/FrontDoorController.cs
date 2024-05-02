using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7) //Checks for player layer
        {
            GetComponentInChildren<Animator>().Play("DoorOpen");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7) //Checks for player layer
        {
            GetComponentInChildren<Animator>().Play("DoorClose");

            GameObject.Find("SequenceController").GetComponent<SequenceManager>().StartCombat();

            GetComponent<BoxCollider>().enabled = false;
            Destroy(this);
        }
    }
}
