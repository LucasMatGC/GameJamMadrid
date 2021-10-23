using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public float TOTAL_TIME = 40f;
    const float GOLD_3D = 80f;
    const float SILVER_3D = 110f;
    const float BRONZE_3D = 150f;

    public AudioSource gameMusic;

    public BeatController beatController;

    public static GameController instance;

    private int notesPressed;
    private int currentScore;
    private int scorePerNote = 100;
    private int scorePerNoteGood = 125;
    private int scorePerNotePerfect = 150;

    private int currentMultiplier;
    private int multiplierTracker;
    public int[] multiplierThreshold;

    public Text end;
    //public GameObject destination;
    //public GameObject player;

    // para saber si se ha iniciado o ha endalizado el juego
    private bool gameRunning;

    private GameTimer timer;

    void Start()
    {
        instance = this;

        timer = FindObjectOfType<GameTimer>();

        end.enabled = false;
        gameRunning = true;
        beatController.hasStarted = true;
        notesPressed = 0;
        currentScore = 0;
        currentMultiplier = 1;
        multiplierTracker = 0;

        gameMusic.Play();
    }

    void Update()
    {
        // jugando
        if (gameRunning)
        {

            // el tiempo ha acabado y no ha llegado al destino
            if (timer.getTimer() <= 0)
            {
                gameRunning = false;
                end.enabled = true;
                end.text = "El tiempo se ha acabado\nPuntuación: 0";
            }
        }
    }

    public void NormalHit()
    {
        currentScore += scorePerNote * currentMultiplier;

        NoteHit();

    }

    public void GoodHit()
    {
        currentScore += scorePerNoteGood * currentMultiplier;

        NoteHit();

    }

    public void PerfectHit()
    {
        currentScore += scorePerNotePerfect * currentMultiplier;

        NoteHit();

    }

    public void NoteHit()
    {

        notesPressed++;
        //currentScore += scorePerNote * currentMultiplier;

        if (currentMultiplier - 1 < multiplierThreshold.Length)
        {

            multiplierTracker++;
            if (multiplierThreshold[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

    }
    public void NoteMissed()
    {

        currentMultiplier = 1;
        multiplierTracker = 0;

    }

    // si hemos llegado al destino
    public void ArriveDestination()
    {
        if (!gameRunning)
            return;

        gameRunning = false;

        // oro
        if ((TOTAL_TIME - timer.getTimer()) < GOLD_3D)
        {
            end.enabled = true;
            end.color = Color.yellow;
            end.text = "Medalla de ORO\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
        }
        // plata
        else if ((TOTAL_TIME - timer.getTimer()) < SILVER_3D && (TOTAL_TIME - timer.getTimer()) >= GOLD_3D)
        {
            end.enabled = true;
            end.color = Color.grey;
            end.text = "Medalle de PLATA\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
        }
        // bronce
        else if ((TOTAL_TIME - timer.getTimer()) < BRONZE_3D && (TOTAL_TIME - timer.getTimer()) >= SILVER_3D)
        {
            end.enabled = true;
            end.color = Color.cyan;
            end.text = "Medalla de BRONCE\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
        }
        // loser
        else
        {
            end.enabled = true;
            end.color = Color.magenta;
            end.text = "Puedes hacerlo mejor\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
        }
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }

    public float getTotalTime()
    {
        return TOTAL_TIME;
    }
}
