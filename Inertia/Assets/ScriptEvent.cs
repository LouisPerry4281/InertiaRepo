using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEvent : MonoBehaviour
{
    ScriptSequenceController sequenceController;

    public int targetSequence;

    private void Start()
    {
        sequenceController = FindAnyObjectByType<ScriptSequenceController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            sequenceController.SetSequence(targetSequence);
            Destroy(gameObject);
        }
    }
}
