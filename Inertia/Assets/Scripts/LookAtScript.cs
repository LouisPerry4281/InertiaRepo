using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    void Update()
    {
        //Constantly faces the camera
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.localScale = new Vector3(-1, 1, 1);
    }
}
