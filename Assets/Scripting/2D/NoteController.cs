using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{

    private bool canBePressed;
    private const float NORMAL_THRESHOLD = 1.0f;
    private const float GOOD_THRESHOLD = 0.5f;

    //public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    public string ButtonAssigned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown(ButtonAssigned) && canBePressed)
        {

            gameObject.SetActive(false);

            //GameController.instance.NoteHit();
            if (Mathf.Abs(transform.position.y) > NORMAL_THRESHOLD)
            {

                GameController.instance.NormalHit();
                //Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);

            } else if (Mathf.Abs(transform.position.y) > GOOD_THRESHOLD)
            {

                GameController.instance.GoodHit();
                //Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);

            } else
            {
                GameController.instance.PerfectHit();
                //Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Button")
        {

            canBePressed = true;

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Button" && gameObject.activeSelf)
        {

            canBePressed = false;

            GameController.instance.NoteMissed();
            //Instantiate(missEffect, transform.position, missEffect.transform.rotation);

        }

    }
}
