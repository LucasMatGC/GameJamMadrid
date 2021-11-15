using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{

    public bool pausedGame;
    public Canvas menuPause;
    public AudioSource music;
    public Texture2D crosshair;
    public GameObject[] menuButtons;

    private string Menu = "MainMenu", CurrentLevel = "FirstLevelMadrid";
    private bool mouseUser = false;
    private Vector3 mouseInitPos;
  
    void Update()
    {

        if (Input.GetButtonDown("MenuButton"))
        {

            pausedGame = !pausedGame;
            menuPause.enabled = pausedGame;
            Time.timeScale = (pausedGame) ? 0 : 1f;
            if (pausedGame){
                Debug.Log("Cycle in menu");
                music.Pause();
                BearHandCursor();
                mouseInitPos = Input.mousePosition;
            }
            else{
                music.Play();
                mouseUser = false;
            }
        }

        if (pausedGame){
            if (EventSystem.current.currentSelectedGameObject == null) EventSystem.current.SetSelectedGameObject(menuButtons[0]);
            if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("DownButton")) && EventSystem.current.currentSelectedGameObject != null){
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            //Inmediatamente deseleccionamos el botón porque como va todo por capas el jugador puede entrar en un bucle infinito
            EventSystem.current.SetSelectedGameObject(null);
            }
            if (!mouseUser && Input.mousePosition != mouseInitPos) mouseUser = true;
        } else EventSystem.current.SetSelectedGameObject(null);

        if (mouseUser) Cursor.visible = true;
        else Cursor.visible = false;
        
    }

    public void Resume ()
    {

        pausedGame = false;
        menuPause.enabled = false;
        Time.timeScale = 1f;
        music.Play();
        mouseUser = false;

    }

    public void Restart()
    {

        menuPause.enabled = false;
        mouseUser = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(CurrentLevel);

    }

    public void LoadMenu()
    {
        mouseUser = false;
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
