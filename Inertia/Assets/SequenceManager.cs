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

    private void Awake()
    {
        waveManager = GetComponent<WaveManager>();
    }

    private void Start()
    {
        currentState = SequenceState.Intro;
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
            enemy.currentStance = BobAI.StanceSelector.Pursuit;
        }

        waveManager.enabled = true;

        currentState = SequenceState.Combat;
    }

    private void EndSequence()
    {
        waveManager.enabled = false;

        print("THIS IS THE END, HOLD YOUR BREATH AND COUNT TO 10, FEEL THE EARTH MOVE");

        //Unlock Final Escape

        //UI Stuff (Get out of there Rye!)

        //Turn on/off any visuals
    }
}
