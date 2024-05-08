using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    public void Sound()
    {
        gameObject.GetComponent<AudioSource>().Play();
        print("play sound");
    }

}
