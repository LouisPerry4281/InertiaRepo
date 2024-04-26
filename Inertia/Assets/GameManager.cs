using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject endUI;

    [SerializeField] float restartTimer;

    public static int enemyCount;

    public static bool hasStartedCombat = false;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    public void KillEnemy(GameObject enemyToKill)
    {
        enemyToKill.GetComponent<BoxCollider>().enabled = false;
        //enemyToKill.GetComponent<BobAI>().enabled = false;
        enemyToKill.GetComponent<EnemyHealth>().enabled = false;
        enemyToKill.GetComponent<NavMeshAgent>().enabled = false;

        Destroy(enemyToKill);

        enemyCount--;

        //Adds 20% juice when an enemy is defeated
        player.GetComponent<PlayerRigidbodyMovement>().JuiceChange(0.4f);
    }

    public void EndGame()
    {
        endUI.SetActive(true);
        player.GetComponent<PlayerRigidbodyMovement>().enabled = false;
        player.GetComponent<PlayerRigidbodyHealth>().enabled = false;
        player.GetComponent<PlayerRigidbodyCombat>().enabled = false;
    }

    public void KillPlayer()
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().isKinematic = true;

        player.GetComponent<PlayerRigidbodyMovement>().enabled = false;
        player.GetComponent<PlayerRigidbodyHealth>().enabled = false;
        player.GetComponent<PlayerRigidbodyCombat>().enabled = false;

        //Turn on ui stuff or whatever

        //DEATH ANIMATION HERE//

        Invoke(nameof(RestartLevel), restartTimer);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
