using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideLevelCheck : MonoBehaviour
{
    public bool isInLevel = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            isInLevel = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            isInLevel = true;
        }
    }
}
