using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public float TOTAL_TIME = 40f;
    const float GOLD_TIME_3D = 90f;
    const float SILVER_TIME_3D = 120f;
    const float BRONZE_TIME_3D = 180f;
    const int GOLD_POINTS_3D = 1500;
    const int SILVER_POINTS_3D = 1000;
    const int BRONZE_POINTS_3D = 500;

    const int GOLD_NOTES_2D = 60;
    const int SILVER_NOTES_2D = 50;
    const int BRONZE_NOTES_2D = 36;
    const int GOLD_POINTS_2D = 1500;
    const int SILVER_POINTS_2D = 1000;
    const int BRONZE_POINTS_2D = 500;

    public AudioSource gameMusic;

    public BeatController beatController;

    public static GameController instance;

    public int totalNotes;
    private int notesPassed;
    private int notesPressed;
    private int currentScore2D;
    private int scorePerNote = 100;
    private int scorePerNoteGood = 125;
    private int scorePerNotePerfect = 150;

    private int totalScore;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThreshold;

    public int[] notesPerStep;
    public int totalAnimation;

    public int currentRecipe;
    public int currentNoteStep;
    public int currentAnimation;

    private Animator animatorArms, animatorHead;


    public Text end;
    public Text multiplier;
    public Image paperEnd;
    //public GameObject player;

    // para saber si se ha iniciado o ha endalizado el juego
    public bool gameRunning3D;
    public bool gameRunning2D;
    public bool showEnd;

    private GameTimer timer;

    void Start()
    {
        instance = this;

        animatorArms = GameObject.Find("OsoBrazos").GetComponent<Animator>();
        animatorHead= GameObject.Find("OsoCabeza").GetComponent<Animator>();

        timer = FindObjectOfType<GameTimer>();

        gameRunning3D = true;
        gameRunning2D = true;
        showEnd = false;
        beatController.hasStarted = true;
        notesPressed = 0;
        currentScore2D = 0;
        currentMultiplier = 1;
        multiplierTracker = 0;

        currentRecipe = 0;
        currentNoteStep = 0;
        currentAnimation = 0;

        multiplier.color = Color.cyan;

        gameMusic.Play();
    }

    void Update()
    {
        // jugando
        if (IsGameRunning())
        {

            // el tiempo ha acabado y no ha llegado al destino
            if (timer.getTimer() <= 0)
            {
                gameRunning3D = false;
                gameRunning2D = false;
            }
        } else
        {
            if (!showEnd)
            {
                showEnd = true;
                EndGame();
            }

        }
    }

    public void NormalHit()
    {
        currentScore2D += scorePerNote * currentMultiplier;

        NoteHit();

    }

    public void GoodHit()
    {
        currentScore2D += scorePerNoteGood * currentMultiplier;

        NoteHit();

    }

    public void PerfectHit()
    {
        currentScore2D += scorePerNotePerfect * currentMultiplier;

        NoteHit();

    }

    public void NoteHit()
    {

        notesPressed++;

        if (currentMultiplier - 1 < multiplierThreshold.Length)
        {

            multiplierTracker++;
            if (multiplierThreshold[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;

                currentMultiplier++;
                multiplier.text = "x" + currentMultiplier;

                if (currentMultiplier == 2)
                    multiplier.color = Color.green;
                else if (currentMultiplier == 3)
                    multiplier.color = Color.yellow;
                else
                    multiplier.color = Color.red;
            }
        }

        end.text = "Puntos: " + currentScore2D;

        ControleStep(true);

    }

    public void NoteMissed()
    {
        currentMultiplier = 1;
        multiplierTracker = 0;
        multiplier.text = "x1";
        multiplier.color = Color.cyan;
        ControleStep(false);

    }

    private void ControleStep(bool didHit)
    {

        currentNoteStep++;

        if (didHit)
        {

            animatorHead.SetTrigger("Happy");

        } else
        {
            animatorHead.SetTrigger("Angry");
        }

        if (currentNoteStep == notesPerStep[currentRecipe])
        {

            currentNoteStep = 0;

            switch (currentAnimation)
            {

                case 0:
                    //coger calamar
                    animatorArms.SetTrigger("TakeRight");
                    break;

                case 1:
                    //cortar clamar
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 2:
                    //coger harina
                    animatorArms.SetTrigger("TakeRight");
                    break;

                case 3:
                    //rebozar calamar
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 4:
                    //freir calamar
                    animatorArms.SetTrigger("ThrowLeft");
                    break;

                case 5:
                    //coger pan
                    animatorArms.SetTrigger("TakeRight");
                    break;

                case 6:
                    //cortar pan
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 7:
                    //Coger mayonesa
                    animatorArms.SetTrigger("TakeRight");
                    break;

                case 8:
                    //poner mayonesa
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 9:
                    //sacar calamar
                    animatorArms.SetTrigger("TakeLeft");
                    break;

                case 10:
                    //meter calamar
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    break;

                default:
                    //montar bocata
                    animatorArms.SetTrigger("Make");
                    animatorArms.SetTrigger("FinishedMaking");
                    //swipe bocata dcha
                    break;

            }

            currentAnimation++;

            if (currentAnimation >= totalAnimation)
            {

                currentAnimation = 0;
                currentRecipe++;

            }

        }

        notesPassed++;

        if (notesPassed == totalNotes)
            gameRunning2D = false;

    }

    // si hemos llegado al destino
    public void ArriveDestination()
    {
        
        gameRunning3D = false;
                
    }

    public bool IsGameRunning()
    {
        return (gameRunning3D || gameRunning2D);
    }

    public bool IsGame3DRunning()
    {
        return gameRunning3D;
    }

    public bool IsGame2DRunning()
    {
        return gameRunning2D;
    }

    public float getTotalTime()
    {
        return TOTAL_TIME;
    }

    private void EndGame()
    {

        if (timer.getTimer() <= 0)
        {
            gameRunning3D = false;
            gameRunning2D = false;
            end.text = "El tiempo se ha acabado\nPuntuación: 0\n¡La proxima vez pisa más el aceBEARador!";
        }
        else
        {

            float minutes = Mathf.FloorToInt(timer.getTimer() / 60);
            float seconds = Mathf.FloorToInt(timer.getTimer() % 60);
            int points3D = (int)timer.getTimer() * 100;
            totalScore = points3D + currentScore2D;

            string results = "Tiempo restante: " + string.Format("{0:00}:{1:00}", minutes, seconds) + " a 100 puntos por segundo: " + points3D;

            //PUNTOS 3D
            // oro
            if ((TOTAL_TIME - timer.getTimer()) < GOLD_TIME_3D)
            {
                end.color = Color.yellow;
                results += "\n¡Medalla de ORO en conducción! (Menos de " + (int)GOLD_TIME_3D + " secs):  " + GOLD_POINTS_3D + " puntos";
                totalScore += GOLD_POINTS_3D;
            }
            // plata
            else if ((TOTAL_TIME - timer.getTimer()) < SILVER_TIME_3D && (TOTAL_TIME - timer.getTimer()) >= GOLD_TIME_3D)
            {

                results += "\n¡Medalla de PLATA en conducción! (Menos de " + (int)SILVER_TIME_3D + " secs):  " + SILVER_POINTS_3D + " puntos";
                totalScore += SILVER_POINTS_3D;

            }
            // bronce
            else if ((TOTAL_TIME - timer.getTimer()) < BRONZE_TIME_3D && (TOTAL_TIME - timer.getTimer()) >= SILVER_TIME_3D)
            {

                results += "\n¡Medalla de BRONCE en conducción! (Menos de " + (int)BRONZE_TIME_3D + " secs):  " + BRONZE_POINTS_3D + " puntos";
                totalScore += BRONZE_POINTS_3D;

            }
            // loser
            else
            {

                results += "\n¡Demasiado lento! No hay medalla en conducción esta vez.";
            }


            //PUNTOS 2D
            //oro
            if (notesPressed > GOLD_NOTES_2D)
            {
                end.color = Color.yellow;
                results += "\n¡Medalla de ORO en cocina! (Más de " + GOLD_NOTES_2D + " secs):  " + GOLD_POINTS_2D + " puntos";
                totalScore += GOLD_POINTS_2D;
            }
            // plata
            else if (notesPressed > SILVER_NOTES_2D)
            {

                results += "\n¡Medalla de PLATA en cocina! (Más de " + SILVER_NOTES_2D + " secs):  " + SILVER_POINTS_2D + " puntos";
                totalScore += SILVER_POINTS_2D;

            }
            // bronce
            else if (notesPressed > BRONZE_NOTES_2D)
            {

                results += "\n¡Medalla de BRONCE en cocina! (Más de " + BRONZE_NOTES_2D + " secs):  " + BRONZE_POINTS_2D + " puntos";
                totalScore += BRONZE_POINTS_2D;

            }
            // loser
            else
            {

                results += "\n¡Demasiado lento! No hay medalla en cocina esta vez.";
            }

            results += "\nPuntuacion total: " + totalScore;

            end.text = results;

            RectTransform endTransform = end.GetComponentInParent<RectTransform>();

            endTransform.transform.SetPositionAndRotation( new Vector3(endTransform.transform.position.x, (endTransform.transform.position.y - 240.0f), endTransform.transform.position.z), endTransform.transform.rotation);
            endTransform.sizeDelta = new Vector2(2000, 400);
            paperEnd.color = new Vector4(paperEnd.color.r, paperEnd.color.g, paperEnd.color.b, 255);

        }

    }
}
