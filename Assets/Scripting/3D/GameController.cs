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

    public Text end;
    //public GameObject destination;
    //public GameObject player;

    // para saber si se ha iniciado o ha endalizado el juego
    private bool gameRunning;

    private GameTimer timer;

    void Start()
    {
        timer = FindObjectOfType<GameTimer>();

        end.enabled = false;
        gameRunning = true;
    }

    void Update()
    {
        // jugando
        if(gameRunning)
        {

            // el tiempo ha acabado y no ha llegado al destino
            if(timer.getTimer() <= 0)
            {
                gameRunning = false;
                end.enabled = true;
                end.text = "El tiempo se ha acabado\nPuntuación: 0";
            }
        }
    }

    // si hemos llegado al destino
    public void ArriveDestination()
    {
        if(!gameRunning)
            return;

        gameRunning = false;

        // oro
        if((TOTAL_TIME - timer.getTimer()) < GOLD_3D)
        {
            end.enabled = true;
            end.color = Color.yellow;
            end.text = "Medalla de ORO\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
        }
        // plata
        else if((TOTAL_TIME - timer.getTimer()) < SILVER_3D && (TOTAL_TIME - timer.getTimer()) >= GOLD_3D)
            {
                end.enabled = true;
                end.color = Color.grey;
                end.text = "Medalle de PLATA\nPuntuación: " + (int)Math.Round(timer.getTimer() * 100);
            }
            // bronce
            else if((TOTAL_TIME - timer.getTimer()) < BRONZE_3D && (TOTAL_TIME - timer.getTimer()) >= SILVER_3D)
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
