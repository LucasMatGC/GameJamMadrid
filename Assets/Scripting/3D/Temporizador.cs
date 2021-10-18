using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Temporizador : MonoBehaviour
{
    const float TIEMPO_TOTAL = 300f;
    const float ORO_CONDUCIR = 80f;
    const float PLATA_CONDUCIR = 110f;
    const float BRONCE_CONDUCIR = 150f;
    
    public Text temporizador;
    public Text fin;
    private float tiempo = TIEMPO_TOTAL;

    // para saber si se ha iniciado o ha finalizado el juego
    private bool gameRunning;   

    void Start()
    {
        gameRunning = true;
        fin.enabled = false;
    }

    void Update()
    {
        // llegar al destino de forma cochina
        if(Input.GetButton("DownButton"))
        {
            gameRunning = false;
        }

        // si estamos jugando
        if(gameRunning)
        {
            if(tiempo > 0)
            { 
                tiempo -= Time.deltaTime; 
            }
            else
            {
                tiempo = 0;
                gameRunning = false;
                fin.enabled = true;
            }

            DisplayTime(tiempo);
        }
        else    // si hemos llegado al destino
        {
            if((TIEMPO_TOTAL - tiempo) < ORO_CONDUCIR)
            {
                fin.enabled = true;
                fin.color = Color.yellow;
                fin.text = "Medalla de ORO\nPuntuación: " + (int)Math.Round(tiempo * 100);
            }
            else if((TIEMPO_TOTAL - tiempo) < PLATA_CONDUCIR && (TIEMPO_TOTAL - tiempo) >= ORO_CONDUCIR)
            {
                fin.enabled = true;
                fin.color = Color.grey;
                fin.text = "Medalle de PLATA\nPuntuación: " + (int)Math.Round(tiempo * 100);
            }
            else if((TIEMPO_TOTAL - tiempo) < BRONCE_CONDUCIR && (TIEMPO_TOTAL - tiempo) >= PLATA_CONDUCIR)
            {
                fin.enabled = true;
                fin.color = Color.cyan;
                fin.text = "Medalla de BRONCE\nPuntuación: " + (int)Math.Round(tiempo * 100);
            }
            else
            {
                fin.enabled = true;
                fin.color = Color.magenta;
                fin.text = "Puedes hacerlo mejor\nPuntuación: " + (int)Math.Round(tiempo * 100);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        temporizador.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool IsGameRunning()
    {
        return gameRunning;
    }
}
