using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour
{
    Animator animator;

    SequenceController sc;

    // 0 - Front Door
    // 1 - Generic Spawn Door
    // 2 - Exit Door
    [SerializeField] int doorType;

    public bool doorReadyToOpen = false;
    bool doorReadyToClose = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sc = FindAnyObjectByType<SequenceController>();
    }

    private void Update()
    {
        switch(doorType)
        {
            case 0:
                FrontDoorLogic();
                break;
            case 1:
                SpawnDoorLogic();
                break;
            case 2:
                ExitDoorLogic();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            doorReadyToOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        doorReadyToClose = true;
    }

    private void FrontDoorLogic()
    {
        if (doorReadyToOpen)
        {
            animator.SetTrigger("DoorOpen");
            sc.IncrementSequence();
            Destroy(this);
        }
    }

    private void SpawnDoorLogic()
    {
        animator.SetTrigger("DoorOpen");
    }

    private void ExitDoorLogic()
    {
        //Null For Now
    }
}
