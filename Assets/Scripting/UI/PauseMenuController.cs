using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public bool pausedGame;
    public Canvas menuPause;
    public AudioSource music;
      public Texture2D crosshair;

    private string Menu = "MainMenu", CurrentLevel = "FirstLevelMadrid";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("MenuButton"))
        {

            pausedGame = !pausedGame;
            menuPause.enabled = pausedGame;
            Time.timeScale = (pausedGame) ? 0 : 1f;
            if (pausedGame){
                music.Pause();
                Cursor.visible = true;
                BearHandCursor();
            }
            else{
                music.Play();
                Cursor.visible = false;
            }
        }

    }

    public void Resume ()
    {

        pausedGame = false;
        menuPause.enabled = false;
        Time.timeScale = 1f;
        music.Play();
        Cursor.visible = false;

    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(CurrentLevel);

    }

    public void LoadMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(Menu);

    }

    public void QuitGame()
    {

        Application.Quit();

    }

    private void BearHandCursor(){
    //Sitúa el origen del cursor a la altura de la punta del boli
    Vector2 cursorOffset = new Vector2(crosshair.width*.35f, crosshair.height*.38f);

    //Cambia el cursor al sprite custom, con su offset y ForceSoftware para saltarse
    //el límite de 32x32 píxeles que tiene windows para los cursores.
    Cursor.SetCursor(crosshair, cursorOffset, CursorMode.ForceSoftware);
  }

}
