using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public bool pausedGame;
    public Canvas menuPause;
    public AudioSource music;

    private string Menu = "MainManu", CurrentLevel = "FirstLevelMadrid";
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
            if (pausedGame)
                music.Pause();
            else
                music.Play();
        }

    }

    public void Resume ()
    {

        pausedGame = false;
        menuPause.enabled = false;
        Time.timeScale = 1f;

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

}
