using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject player;

    GameManager gm;

    private void Start()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //When player enters end zone, end the game
        if (other.gameObject.layer == 7)
        {
            gm.EndGame();
        }
    }
}
