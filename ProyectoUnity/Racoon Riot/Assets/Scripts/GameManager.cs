using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menu;
    int escCount = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && escCount == 0)
        {
            menu.SetActive(true);
            escCount = 1;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && escCount == 1)
        {
            menu.SetActive(false);
            escCount = 0;
            Time.timeScale = 1;
        }
    }
}
