using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private float timer;

    private bool timerStarted;
    private bool timerEnded;
    
    public Text timer_text;

    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        timer = gameController.getTotalTime();
        timerStarted = true;
        timerEnded = false;
    }

    void Update()
    {
    /*  // llegar al destino de forma cochina
        if(Input.GetButton("DownButton"))
        {
            gameRunning = false;
        }
    */
        // si estamos jugando
        if(gameController.IsGameRunning())
        {
            if(timer > 0)
            { 
                timer -= Time.deltaTime; 
            }
            else
            {
                timer = 0;
                timerEnded = true;
            }

            Displaytimer(timer);
        }
        else
        {
            timerEnded = true;
        }
    }

    void Displaytimer(float timerToDisplay)
    {
        if(timerToDisplay < 0)
        {
            timerToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timerToDisplay / 60);
        float seconds = Mathf.FloorToInt(timerToDisplay % 60);

        timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool didTimerStart()
    {
        return timerStarted;
    } 

    public bool didTimerEnd()
    {
        return timerEnded;
    }

    public float getTimer()
    {
        return timer;
    }
}
