using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SequenceManager : MonoBehaviour
{
    public enum SequenceState
    {
        Intro,
        Combat,
        EndOpen,
        End
    }

    public SequenceState currentState;

    private void Update()
    {
        switch (currentState) //Switches which "State" is used depending on what the pointer has selected
        {
            case SequenceState.Intro:
                IntroState();
                break;
            case SequenceState.Combat:
                CombatState();
                break;
            case SequenceState.EndOpen:
                EndOpenState();
                break;
            case SequenceState.End:
                EndState();
                break;
        }
    }

    public List<BobAI> enemies = new List<BobAI>(); //Stores a list of enemies currently alive

    [SerializeField] int enemiesLeftForEnd;

    WaveManager waveManager;

    [SerializeField] GameObject finalInvisWall;
    [SerializeField] Animator finalDoor;

    [SerializeField] GameObject alarmParent;

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        currentState = SequenceState.Intro;

        finalDoor.Play("DoorClose");
    }

    void IntroState()
    {

    }

    void CombatState()
    {
        if (waveManager.isFinalWave && waveManager.enemiesAlive <= enemiesLeftForEnd)
        {
            EndSequence();
            currentState = SequenceState.EndOpen;
        }
    }

    void EndOpenState()
    {

    }

    void EndState()
    {

    }

    public void StartCombat()
    {
        //Grabs all enemies that start in the scene, and stores them in an array
        BobAI[] enemyArray = GameObject.FindObjectsByType<BobAI>(FindObjectsSortMode.None);
        
        for(var i = 0; i < enemyArray.Length; i++) //Moves the array entries into a list
        {
            enemies.Add(enemyArray[i]);
        }


        foreach(BobAI enemy in enemies) //Sets all enemies to active
        {
            GameManager.hasStartedCombat = true;
        }

        waveManager.enabled = true;

        currentState = SequenceState.Combat;

        alarmParent.SetActive(true);

        AudioManager.instance.PlaySFX("IntruderAlert", 1, 1);
        AudioManager.instance.PlayMusic("Glow", 1, 1);
        //AudioManager.instance.PlayMusic("FactoryNoise"); Not currently active, as the audio manager doesnt support multiple sounds yet.
        Invoke("Siren", 1f);
    }

    private void EndSequence()
    {
        waveManager.enabled = false;

        finalInvisWall.SetActive(false);
        finalDoor.Play("DoorOpen");

        //UI Stuff (Get out of there Rye!)
        GameObject.Find("DialogueBox").GetComponent<Animator>().SetTrigger("Trigger");
        Invoke("DialogueBox", 6f);

        //Turn on/off any visuals
    }

    void Siren() //Play's the Siren Noise every 8 seconds.
    {
        AudioManager.instance.PlaySFX("Siren", 1, 1);
        Invoke("Siren", 8f);
    }

    void DialogueBox()
    {
        GameObject.Find("DialogueBox").GetComponent<Animator>().SetTrigger("Trigger");
    }



}
