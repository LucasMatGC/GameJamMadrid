using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public string KeyButton;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown(KeyButton))
        {

            spriteRenderer.sprite = pressedImage;

        }

        if (Input.GetButtonUp(KeyButton))
        {
            spriteRenderer.sprite = defaultImage;

        }

    }
}
