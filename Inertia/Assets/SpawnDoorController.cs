using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDoorController : MonoBehaviour
{
    Animator anim;

    bool doorClosed;

    int enemiesInside;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        CloseCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) //Enemy Layer
        {
            enemiesInside++;

            CloseCheck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6) //Enemy Layer
        {
            enemiesInside--;

            CloseCheck();
        }
    }

    private void CloseCheck()
    {
        if (enemiesInside <= 0 && !doorClosed)
        {
            doorClosed = true;

            anim.Play("DoorClose");
        }

        if (enemiesInside > 0 && doorClosed)
        {
            doorClosed = false;

            anim.Play("DoorOpen");
        }
    }
}
