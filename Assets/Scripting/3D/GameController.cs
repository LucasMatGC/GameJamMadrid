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

    public static GameController instance;

    public AudioSource gameMusic;
    public AudioSource gameWin;
    public AudioSource gameLose;

    public BeatController beatController;

    public int totalNotes;
    private int notesPassed;
    private int notesPressed;
    private int currentScore2D;
    private int scorePerNote = 100;
    private int scorePerNoteGood = 125;
    private int scorePerNotePerfect = 150;

    private int totalScore;
    private float timeEndDrive = 0.0f;

    private int currentMultiplier;
    private int multiplierTracker;
    public int[] multiplierThreshold;

    public int[] notesPerStep;
    public int totalAnimation;

    private int currentRecipe;
    private int currentNoteStep;
    private int currentAnimation;

    //ANIMATIONS BEAR
    private Animator animatorArms, animatorHead;

    //ANIMATOR FREIDORA
    public Animator fryer;

    //ANIMATOR PUERTA
    public Animator door;

    //GAMEOBJECTS INGREDIENTS
    public GameObject[] itemsAppear;
    public GameObject[] stepGood;
    public GameObject[] stepBad;
    private GameObject[] toDisappear;
    public GameObject particleGood;
    public GameObject particleBad;
    private GameObject previousStep;

    private bool niceCut;
    private bool niceSwipe;

    public Text end;
    public Text multiplier;
    public Text upsideDown;
    public Text end3DMessage;
    public Text end2DMessage;
    public Text counter;
    public Image paperEnd;
    //public GameObject player;

    // para saber si se ha iniciado o ha endalizado el juego
    private bool gameRunning3D;
    private bool gameRunning2D;
    private bool showEnd;
    private bool pausedGame;

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

        StartCoroutine(StartRecipe());
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
                    itemsAppear[currentAnimation].SetActive(false);
                    animatorArms.SetTrigger("TakeRight");
                    //stepGood[currentAnimation].SetActive(true);
                    StartCoroutine(ActivateItem(stepGood[currentAnimation], 0.3f));
                    break;

                case 1:
                    //cortar clamar
                    animatorArms.SetTrigger("Make");

                    toDisappear = new GameObject[] {stepGood[currentAnimation - 1]};
                    if (didHit)
                    {
                        //stepGood[currentAnimation].SetActive(true);
                        StartCoroutine(ActivateItem(stepGood[currentAnimation], 0.5f, toDisappear));

                        previousStep = stepGood[currentAnimation];
                        niceCut = true;
                    }
                    else
                    {
                        //stepBad[currentAnimation - 1].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 1], 0.5f, toDisappear));

                        previousStep = stepBad[currentAnimation - 1];
                        niceCut = false;
                    }

                    door.SetTrigger("OpenDoor");
                    itemsAppear[currentAnimation].SetActive(true);

                    StartCoroutine(EndMakeAnimation(didHit));
                    //animatorArms.SetTrigger("FinishedMaking");
                    
                    break;

                case 2:
                    //coger harina
                    itemsAppear[currentAnimation - 1].SetActive(false);
                    animatorArms.SetTrigger("TakeRight");
                    //Colocar harina en tabla
                    StartCoroutine(ActivateItem(stepGood[currentAnimation + 8], 0.5f));
                    break;

                case 3:
                    //rebozar calamar
                    animatorArms.SetTrigger("Make");

                    toDisappear = new GameObject[] {stepGood[currentAnimation + 7], previousStep};
                    if (niceCut)
                    {
                        //stepGood[currentAnimation - 1].SetActive(true);
                        StartCoroutine(ActivateItem(stepGood[currentAnimation - 1], 0.5f, toDisappear));

                        previousStep = stepGood[currentAnimation - 1];
                    }
                    else
                    {
                        //stepBad[currentAnimation - 2].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 2], 0.5f, toDisappear));

                        previousStep = stepBad[currentAnimation - 2];
                    }

                    StartCoroutine(EndMakeAnimation(didHit));
                    //animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 4:
                    //freir calamar
                    previousStep.SetActive(false);
                    animatorArms.SetTrigger("ThrowLeft");
                    fryer.SetTrigger("Friendo");
                    door.SetTrigger("OpenDoor");
                    itemsAppear[currentAnimation - 2].SetActive(true);
                    break;

                case 5:
                    //coger pan
                    itemsAppear[currentAnimation - 3].SetActive(false);
                    animatorArms.SetTrigger("TakeRight");
                    StartCoroutine(ActivateItem(stepGood[currentAnimation - 2], 0.3f));
                    previousStep = stepGood[currentAnimation - 2];
                    break;

                case 6:
                    //cortar pan
                    animatorArms.SetTrigger("Make");

                    toDisappear = new GameObject[] {previousStep};
                    if (didHit)
                    {
                        //stepGood[currentAnimation - 2].SetActive(true);
                        StartCoroutine(ActivateItem(stepGood[currentAnimation - 2], 0.5f, toDisappear));
                        previousStep = stepGood[currentAnimation - 2];
                    }
                    else
                    {
                        //stepBad[currentAnimation - 4].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 4], 0.5f, toDisappear));
                        previousStep = stepBad[currentAnimation - 4];
                    }

                    door.SetTrigger("OpenDoor");
                    itemsAppear[currentAnimation - 3].SetActive(true);

                    StartCoroutine(EndMakeAnimation(didHit));
                    //animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 7:
                    //Coger mayonesa
                    itemsAppear[currentAnimation - 4].SetActive(false);
                    animatorArms.SetTrigger("TakeRight");

                    //Colocar mayonesa en tabla
                    StartCoroutine(ActivateItem(stepGood[currentAnimation + 2], 0.5f));
                    break;

                case 8:
                    //poner mayonesa

                    animatorArms.SetTrigger("Make");
                    
                    toDisappear = new GameObject[] {stepGood[currentAnimation + 1], previousStep};
                    if (didHit)
                    {

                        //stepGood[currentAnimation - 3].SetActive(true);
                        StartCoroutine(ActivateItem(stepGood[currentAnimation - 3], 0.5f, toDisappear));
                        previousStep = stepGood[currentAnimation - 3];
                        niceSwipe = true;

                    } else
                    {

                        //stepBad[currentAnimation - 5].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 5], 0.5f, toDisappear));
                        previousStep = stepBad[currentAnimation - 5];
                        niceSwipe = false;

                    }

                    //Quitar mayonesa
                    StartCoroutine(EndMakeAnimation(didHit));
                    //animatorArms.SetTrigger("FinishedMaking");
                    break;

                case 9:
                    //sacar calamar
                    animatorArms.SetTrigger("TakeLeft");
                    fryer.SetTrigger("NoFriendo");

                    //Poner calamar frito en mesa
                    if (niceCut){
                        StartCoroutine(ActivateItem(stepGood[currentAnimation-1], 0.5f));
                    }
                    else StartCoroutine(ActivateItem(stepBad[currentAnimation-1], 0.5f));
                    break;

                case 10:
                    //meter calamar
                    animatorArms.SetTrigger("Make");

                    //Ocultar calamar frito
                    if (stepGood[currentAnimation-2].activeInHierarchy) toDisappear = new GameObject[] {stepGood[currentAnimation-2], previousStep};
                    else toDisappear = new GameObject[] {stepBad[currentAnimation-2], previousStep};

                    if (niceCut)
                    {
                        if (niceSwipe)
                        {

                            //stepGood[currentAnimation - 4].SetActive(true);
                            StartCoroutine(ActivateItem(stepGood[currentAnimation - 4], 0.5f, toDisappear));
                            previousStep = stepGood[currentAnimation - 4];

                        } else
                        {

                            //stepBad[currentAnimation - 5].SetActive(true);
                            StartCoroutine(ActivateItem(stepBad[currentAnimation - 5], 0.5f, toDisappear));
                            previousStep = stepBad[currentAnimation - 5];

                        }

                    } else if (niceSwipe)
                    {

                        //stepBad[currentAnimation - 6].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 6], 0.5f, toDisappear));
                        previousStep = stepBad[currentAnimation - 6];

                    } else
                    {

                        //stepBad[currentAnimation - 4].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 4], 0.5f, toDisappear));
                        previousStep = stepBad[currentAnimation - 4];

                    }

                    StartCoroutine(EndMakeAnimation(didHit));
                    //animatorArms.SetTrigger("FinishedMaking");
                    break;

                default:
                    //montar bocata
                    animatorArms.SetTrigger("Make");

                    
                    toDisappear = new GameObject[] {previousStep};
                    if (niceCut)
                    {

                        //stepGood[currentAnimation - 4].SetActive(true);
                        StartCoroutine(ActivateItem(stepGood[currentAnimation - 4], 0.5f, toDisappear));
                        previousStep = stepGood[currentAnimation - 4];

                    }
                    else
                    {

                        //stepBad[currentAnimation - 4].SetActive(true);
                        StartCoroutine(ActivateItem(stepBad[currentAnimation - 4], 0.5f, toDisappear));
                        previousStep = stepBad[currentAnimation - 4];

                    }

                    StartCoroutine(EndMakeAnimation(didHit));

                    StartCoroutine(RemoveFinalItem(previousStep));
                    StartCoroutine(StartRecipe());

                    //animatorArms.SetTrigger("FinishedMaking");
                    //swipe bocata dcha
                    break;

            }

            currentAnimation++;

            if (currentAnimation >= totalAnimation)
            {

                currentAnimation = 0;
                currentRecipe++;
                counter.text = currentRecipe + "/3";

            }

        }

        notesPassed++;

        if (notesPassed == totalNotes)
        {

            gameRunning2D = false;
            end2DMessage.enabled = true;

        }

    }

    private IEnumerator EndMakeAnimation(bool didHit)
    {
        yield return new WaitForSeconds(1);
        animatorArms.SetTrigger("FinishedMaking");
        
        if (didHit) particleGood.GetComponent<ParticleSystem>().Play();
        else particleBad.GetComponent<ParticleSystem>().Play();

    }

    private IEnumerator ActivateItem(GameObject item, float time, GameObject[] toDisappear = null)
    {
        yield return new WaitForSeconds(time);
        item.SetActive(true);
        if (toDisappear != null){
            foreach (GameObject ingredient in toDisappear) ingredient.SetActive(false);
        }

    }

    private IEnumerator RemoveFinalItem(GameObject finalIngredient)
    {
        yield return new WaitForSeconds(3);
        previousStep.SetActive(false);
        animatorArms.SetTrigger("ThrowLeft");
        yield return new WaitForSeconds(0.5f);
        finalIngredient.SetActive(false);

    }

    private IEnumerator StartRecipe()
    {
        yield return new WaitForSeconds(2);
        door.SetTrigger("OpenDoor");
        itemsAppear[0].SetActive(true);

    }


    // si hemos llegado al destino
    public void ArriveDestination()
    {
        
        gameRunning3D = false;
        end3DMessage.enabled = true;
        timeEndDrive = timer.getTimer();
                
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

        end3DMessage.enabled = false;
        end2DMessage.enabled = false;

        if (timer.getTimer() <= 0)
        {
            gameRunning3D = false;
            gameRunning2D = false;
            end.text = "El tiempo se ha acabado\nPuntuación: 0\n¡La proxima vez pisa más el aceBEARador!";
            gameMusic.Stop();
            gameLose.Play();
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
            if ((TOTAL_TIME - timeEndDrive) < GOLD_TIME_3D)
            {
                end.color = Color.yellow;
                results += "\n¡Medalla de ORO en conducción! (Menos de " + (int)GOLD_TIME_3D + " secs):  " + GOLD_POINTS_3D + " puntos";
                totalScore += GOLD_POINTS_3D;
            }
            // plata
            else if ((TOTAL_TIME - timeEndDrive) < SILVER_TIME_3D && (TOTAL_TIME - timeEndDrive) >= GOLD_TIME_3D)
            {

                results += "\n¡Medalla de PLATA en conducción! (Menos de " + (int)SILVER_TIME_3D + " secs):  " + SILVER_POINTS_3D + " puntos";
                totalScore += SILVER_POINTS_3D;

            }
            // bronce
            else if ((TOTAL_TIME - timeEndDrive) < BRONZE_TIME_3D && (TOTAL_TIME - timeEndDrive) >= SILVER_TIME_3D)
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

                results += "\n¡Demasiados fallos! No hay medalla en cocina esta vez.";
            }

            results += "\nPuntuacion total: " + totalScore;

            end.text = results;


            //endTransform.transform.SetPositionAndRotation( new Vector3(endTransform.transform.position.x, (endTransform.transform.position.y - 240.0f), endTransform.transform.position.z), endTransform.transform.rotation);
            gameMusic.Stop();
            gameWin.Play();

        }

        RectTransform endTransform = end.GetComponentInParent<RectTransform>();
        RectTransform paperTransform = paperEnd.GetComponentInParent<RectTransform>();

        endTransform.transform.SetPositionAndRotation(new Vector3(0, 0, 0), endTransform.transform.rotation);
        endTransform.sizeDelta = new Vector2(2000, 400);
        paperTransform.transform.SetPositionAndRotation(new Vector3(-2, 0, 0), paperTransform.transform.rotation);
        paperTransform.sizeDelta = new Vector2(550, 300);
    }

    public void ShowUpsideDownText(bool isActive)
    {

        upsideDown.enabled = isActive;

    }

}