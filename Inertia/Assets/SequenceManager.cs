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

    [SerializeField] GameObject text4;
    [SerializeField] GameObject text3;
    [SerializeField] GameObject text2;
    [SerializeField] GameObject text1;

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
        AudioManager.instance.PlayMusic("Glow", 0.4f, 1, false);
        AudioManager.instance.PlayMusic("FactoryNoise", 0.1f, 1, true);
        Invoke("Siren", 1f);
    }

    private void EndSequence()
    {
        waveManager.enabled = false;

        finalInvisWall.SetActive(false);
        finalDoor.Play("DoorOpen");

        //UI Stuff (Get out of there Rye!)
        GameObject.Find("DialogueBox").GetComponent<Animator>().SetTrigger("Trigger");
        text4.SetActive(true);
        Invoke("VoiceLine4", 1f);
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

    void VoiceLine4()
    {
        AudioManager.instance.PlaySFX("ELI4", 1.1f, 1);
        Invoke("HangUp", 4f);
    }

    void HangUp()
    {
        AudioManager.instance.PlaySFX("HangUp", 1.2f, 1);
    }



}
