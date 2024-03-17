using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    Animator animator;

    ScriptSequenceController sequenceController;

    private void Start()
    {
        sequenceController = FindAnyObjectByType<ScriptSequenceController>();

        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            animator.SetTrigger("Close");
        }
    }
}
