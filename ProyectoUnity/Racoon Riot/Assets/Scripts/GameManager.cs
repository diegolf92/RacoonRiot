using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public EventSystem eventSystem;
    public GameObject menu;
    public GameObject firstSelectedPause;

    int escCount = 0;
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(firstSelectedPause);
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && escCount == 0)
        {
            eventSystem.SetSelectedGameObject(firstSelectedPause);
            menu.SetActive(true);
            escCount = 1;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && escCount == 1)
        {
            menu.SetActive(false);
            escCount = 0;
        }
    }
}
