using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class StaticCamController : MonoBehaviour
{
    /*[SerializeField] CinemachineFreeLook freeLookCam;
    bool isStatic = false;

    private void Update()
    {
        FollowPlayer();

        if (Input.GetKeyDown(KeyCode.LeftShift) && isStatic)
        {
            UnstickCamera();
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift) && !isStatic)
        {
            StickCamera();
        }
    }

    private void FollowPlayer()
    {
    
    }

    public void StickCamera()
    {
        print("Stick");
        isStatic = true;

        freeLookCam.Priority = 0;
        transform.parent = null;
    }

    public void UnstickCamera()
    {
        print("Unstick");
        isStatic = false;

        freeLookCam.Priority = 10;
        transform.parent = freeLookCam.transform;
        transform.position = freeLookCam.transform.position;
    }
    */
}
