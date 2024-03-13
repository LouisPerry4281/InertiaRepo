using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanup : MonoBehaviour
{
    [SerializeField] float cleanupTimer;

    void Start()
    {
        Invoke("DestroyMe", cleanupTimer);
    }

    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
