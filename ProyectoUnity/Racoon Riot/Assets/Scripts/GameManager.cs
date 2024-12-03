using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject menu;
    public GameObject[] firstSelectedPause;
    public int menuAbierto;

    int escCount = 0;

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(firstSelectedPause[menuAbierto]);
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && escCount == 0)
        {
            Time.timeScale = 0f;
            eventSystem.SetSelectedGameObject(firstSelectedPause[menuAbierto]);
            menu.SetActive(true);
            escCount = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && escCount == 1)
        {
            Time.timeScale = 1f;
            menu.SetActive(false);
            escCount = 0;
        }
    }

    public void GetMenuOpenNumber(int menu)
    {
        menuAbierto = menu;
    }
}
