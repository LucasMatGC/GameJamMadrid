using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetiveController : MonoBehaviour
{
    private GameController gameController;
    
    void Start()
    {
        //player = GameObject.Find("Player3D");
        gameController = GameController.instance;
    }

    void Update()
    {
        
    }

    // si hemos llegado al destino
    void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Player")
       {
            gameController.ArriveDestination();

            /*Renderer render = GetComponent<Renderer>();
            render.material.color = Color.green;*/
       }
               
    }
}
