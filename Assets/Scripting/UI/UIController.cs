using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
  public Texture2D crosshair;
  public GameObject turnPageLayer;
  public GameObject[] canvasArray, pressAnyCanvases, menuButtons;

  private GameObject currentCanvas;
  private bool isLoading = false;

  void Start(){
    currentCanvas = canvasArray[0];
  }

  private void Update(){
    //Comprueba si estás en un menú en el que haya que pulsar lo que sea para continuar,
    //y te manda al sitio pertinente.
    if (Array.Exists(pressAnyCanvases, el => el == currentCanvas) && Input.anyKeyDown){
      switch (currentCanvas.name){
        case "IntroScreen":
          ChangeCanvasBtn(canvasArray[1]);
          break;
        case "Objective1":
          ChangeCanvasBtn(canvasArray[3]);
          break;
        case "Objective2":
          isLoading = true;
          StartCoroutine(LoadGameScene());
          break;
        case "Controller":
          ChangeCanvasBtn(canvasArray[5]);
          break;
        case "Credits":
          if(currentCanvas.gameObject.transform.GetChild(1).gameObject.activeInHierarchy) ChangeCanvasBtn(canvasArray[1]);
          break;
        default:
          ChangeCanvasBtn(canvasArray[1]);
          break;
      }
    }
    if (currentCanvas != canvasArray[1]) EventSystem.current.SetSelectedGameObject(null);
    else if (EventSystem.current.currentSelectedGameObject == null && Mathf.Abs(Input.GetAxis("VerticalUI")) >= 0.2f) EventSystem.current.SetSelectedGameObject(menuButtons[0]);
    if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("DownButton")) && EventSystem.current.currentSelectedGameObject != null){
      EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
      //Inmediatamente deseleccionamos el botón porque como va todo por capas el jugador puede entrar en un bucle infinito
      EventSystem.current.SetSelectedGameObject(null);
    }

    if (currentCanvas == canvasArray[6] || isLoading){
      Cursor.visible = false;
    }
    else{
      Cursor.visible = true;
      BearHandCursor();
    }
  }

  //Función para cambiar de menú. Llamada desde los botones del menú y el script
  public void ChangeCanvasBtn(GameObject nextCanvas){
    turnPageLayer.GetComponent<Animator>().SetTrigger("PageTurn");
    StartCoroutine(LoadPage(nextCanvas));
  }

  public void CreditsBtn(){
    ChangeCanvasBtn(canvasArray[6]);
  }

  public void ExitBtn(){
    Application.Quit();
  }

  //Sincroniza el paso de página con la carga del siguiente menú.
  private IEnumerator LoadPage(GameObject nextCanvas)
 {
   yield return new WaitForSeconds(.25f);

   currentCanvas.SetActive(false);
   nextCanvas.SetActive(true);
   currentCanvas = nextCanvas;

   if (currentCanvas.name == "Credits"){
     currentCanvas.gameObject.transform.GetChild(1).gameObject.SetActive(false);
     yield return new WaitForSeconds(5);
     currentCanvas.gameObject.transform.GetChild(1).gameObject.SetActive(true);
   }
 }

  private IEnumerator LoadGameScene(){
        // Carga la siguiente escena mientras el resto del código carga. La carga es prácticamente instantánea.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("FirstLevelMadrid");

        // Espera hasta que la escena esté cargada
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
  }

  private void BearHandCursor(){
    //Sitúa el origen del cursor a la altura de la punta del boli
    Vector2 cursorOffset = new Vector2(crosshair.width*.35f, crosshair.height*.38f);

    //Cambia el cursor al sprite custom, con su offset y ForceSoftware para saltarse
    //el límite de 32x32 píxeles que tiene windows para los cursores.
    Cursor.SetCursor(crosshair, cursorOffset, CursorMode.ForceSoftware);
  }

}
