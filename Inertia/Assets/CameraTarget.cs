using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = GameObject.Find("PlayerCameraRoot").GetComponent<Transform>();
    }

    private void Update()
    {
        transform.position = target.position;
    }
}
