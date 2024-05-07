using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotate : MonoBehaviour
{
    float xRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(50 * Time.deltaTime, 100 * Time.deltaTime, 0);


    }
}
