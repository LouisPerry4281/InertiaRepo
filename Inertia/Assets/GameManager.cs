using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    SequenceManager sequenceManager;

    GameObject player;
    [SerializeField] GameObject endUI;
    [SerializeField] GameObject deathUI;
    [SerializeField] GameObject juiceUI;
    Animator anim;

    [SerializeField] float restartTimer;

    bool isDead = false;

    public static int enemyCount;

    public static bool hasStartedCombat = false;

    private void Start()
    {
        player = GameObject.Find("Player");
        sequenceManager = FindAnyObjectByType<SequenceManager>();
        anim = player.GetComponentInChildren<Animator>();
    }

    public void KillEnemy(GameObject enemyToKill, bool shouldGiveJuice)
    {
        enemyToKill.GetComponent<BoxCollider>().enabled = false;
        //enemyToKill.GetComponent<BobAI>().enabled = false;
        enemyToKill.GetComponent<EnemyHealth>().enabled = false;
        enemyToKill.GetComponent<NavMeshAgent>().enabled = false;

        CinemachineShake.Instance.ShakeCamera(6, .2f);

        AudioManager.instance.PlaySFX("RobotDestroy", 1, 1.5f);

        Destroy(enemyToKill);

        enemyCount--;

        if (shouldGiveJuice)
        {
            //Adds 20% juice when an enemy is defeated
            player.GetComponent<PlayerRigidbodyMovement>().JuiceChange(0.4f);
        }

        sequenceManager.enemies.Remove(enemyToKill.GetComponent<BobAI>());
    }

    public void EndGame()
    {
        endUI.SetActive(true);
        player.GetComponent<PlayerRigidbodyMovement>().enabled = false;
        player.GetComponent<PlayerRigidbodyHealth>().enabled = false;
        player.GetComponent<PlayerRigidbodyCombat>().enabled = false;

        player.GetComponentInChildren<Animator>().SetFloat(0, 0);
    }

    public void KillPlayer()
    {
        if (isDead)
            return;

        isDead = true;

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().isKinematic = true;

        player.GetComponent<PlayerRigidbodyMovement>().enabled = false;
        player.GetComponent<PlayerRigidbodyHealth>().enabled = false;
        player.GetComponent<PlayerRigidbodyCombat>().enabled = false;

        juiceUI.SetActive(false);
        deathUI.SetActive(true);

        //DEATH ANIMATION HERE//
        anim.Play("Death");

        Invoke(nameof(RestartLevel), restartTimer);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
