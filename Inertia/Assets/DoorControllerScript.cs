using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour
{
    Animator animator;

    // 0 - Front Door
    // 1 - Generic Spawn Door
    // 2 - Exit Door
    [SerializeField] int doorType;

    bool doorReadyToOpen = false;
    bool doorReadyToClose = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
        doorReadyToOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        doorReadyToClose = true;
    }

    private void FrontDoorLogic()
    {
        if (doorReadyToOpen)
        {
            animator.SetTrigger("OpenDoor");
            SequenceController.IncrementSequence();
            
        }
    }

    private void SpawnDoorLogic()
    {

    }

    private void ExitDoorLogic()
    {

    }
}
