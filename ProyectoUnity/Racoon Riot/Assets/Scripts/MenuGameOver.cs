using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
public class MenuGameOver: MonoBehaviour
{
    public Button firstButton;
    private GameObject lastSelected;
    public EventSystem eventSystem;
    public int menuAbierto;
    public GameManager gameManager;

    void Start()
    {
        gameManager.GetMenuOpenNumber(menuAbierto);
        // Set the initial button selection
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    private void Update()
    {

    }

    public void Continuar()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ControlsOpened()
    {
        gameManager.GetMenuOpenNumber(3);
    }

    public void ControlsClosed()
    {
        gameManager.GetMenuOpenNumber(0);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if the next scene index is within the available scene count
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
