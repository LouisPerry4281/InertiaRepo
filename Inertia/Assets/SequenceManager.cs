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
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject tasks;
    [SerializeField] GameObject RyeText1;
    [SerializeField] GameObject RyeText2;
    [SerializeField] GameObject RyeText3;

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        currentState = SequenceState.Intro;

        finalDoor.Play("DoorClose");

        StartCoroutine(StartSequence());

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
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");
        text4.SetActive(true);
        Invoke("VoiceLine4", 1f);
        Invoke("DialogueBox", 6f);

        //Turn on/off any visuals
    }



    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(1f); //Wait Timer From Start of Game
        AudioManager.instance.PlaySFX("Ring", 0.2f, 1);

        yield return new WaitForSeconds(4.5f); //Wait for Ringing to end


        text1.SetActive(true); //Sets first dialogue text object to active
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); //Trigger the Dialogue Box on for the 1st voiceline
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("ELI1", 0.8f, 1); //Play the voiceline
        yield return new WaitForSeconds(4f);
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); //Turn Off the Dialogue Box

        yield return new WaitForSeconds(1f); //Wait Timer Between 2nd Dialogue


        text1.SetActive(false); //Sets ELI's first dialogue text to inactive
        RyeText1.SetActive(true); //Sets Ryes 1st dialogue text to active
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); // RYE's 1st bit of Dialogue
        yield return new WaitForSeconds(1f);
        //AudioManager.instance.PlaySFX("RYE1", 1f, 1); <------- PLAY RYE's FIRST VOICELINE HERE
        yield return new WaitForSeconds(4f);
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");

        yield return new WaitForSeconds(1f); //Wait Timer Between 3rd Dialogue

        RyeText1.SetActive(false); //Sets Rye's first dialogue text to inactive
        text2.SetActive(true); //Sets ELI's second dialogue text to active
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); // ELI's 2nd bit of Dialogue
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("ELI2", 0.8f, 1);
        yield return new WaitForSeconds(2.5f);
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");

        yield return new WaitForSeconds(1f); //Wait Timer Between 4th Dialogue

        text2.SetActive(false); //Sets ELI's second text to false
        RyeText2.SetActive(true); //Sets Rye's second text to true
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); // RYE's 2nd bit of Dialogue
        yield return new WaitForSeconds(1f);
        //AudioManager.instance.PlaySFX("RYE2", 1f, 1); <------- PLAY RYE's SECOND VOICELINE HERE
        yield return new WaitForSeconds(4f);
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");

        yield return new WaitForSeconds(1f); //Wait Timer Between 5th Dialogue

        text3.SetActive(true); //Sets ELI's third text to true
        RyeText2.SetActive(false); //Sets Rye's second text to false
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger"); // ELI's 3rd bit of Dialogue
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX("ELI3", 0.8f, 1);
        yield return new WaitForSeconds(0.75f);
        AudioManager.instance.PlaySFX("HangUp", 1.2f, 1); //Plays the Hang Up noise as well.
        yield return new WaitForSeconds(0.3f);
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");

        yield return new WaitForSeconds(1f);
        text3.SetActive(false); //Sets ELI's third text back to false
        tasks.SetActive(true); //Sets the Task list to active

        yield return null;
    }


    void Siren() //Play's the Siren Noise every 8 seconds.
    {
        AudioManager.instance.PlaySFX("Siren", 1, 1);
        Invoke("Siren", 8f);
    }

    void DialogueBox()
    {
        dialogueBox.GetComponent<Animator>().SetTrigger("Trigger");
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
