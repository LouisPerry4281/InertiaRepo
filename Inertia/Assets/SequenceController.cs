using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour
{
    // 0 - Before Combat
    // 1 - During Combat
    // 2 - Close To Done
    // 3 - Exit Unlocked
    public static int sequenceIndex = 0;

    GameObject endTrigger;

    [SerializeField] GameObject frontDoor;

    [SerializeField] GameObject bay1Door;
    [SerializeField] GameObject bay2Door;
    [SerializeField] GameObject bay3Door;

    private void Start()
    {
        endTrigger = FindAnyObjectByType<EndTrigger>().gameObject;
        endTrigger.SetActive(false);
    }

    public void IncrementSequence()
    {
        sequenceIndex++;

        if (sequenceIndex >= 4)
        {
            endTrigger.SetActive(true);
        }

        Invoke(nameof(StartCombat), 1.0f);
    }

    void StartCombat()
    {
        GameManager.hasStartedCombat = true;
    }
}
