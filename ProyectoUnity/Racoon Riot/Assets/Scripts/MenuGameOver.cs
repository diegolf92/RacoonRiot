using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuGameOver: MonoBehaviour
{
    public Button firstButton;
    private GameObject lastSelected;

    void Start()
    {
        // Set the initial button selection
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }

    private void Update()
    {
        {
            // Get the currently selected GameObject
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            // Check for arrow key inputs to navigate between buttons
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateToButton(-1);  // Navigate to the previous button
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateToButton(1);   // Navigate to the next button
            }

            // Detect "Space" or "Enter" key press for button click
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Button selectedButton = currentSelected?.GetComponent<Button>();
                if (selectedButton != null)
                {
                    selectedButton.onClick.Invoke(); // Trigger the button click
                }
            }
        }
    }

    private void NavigateToButton(int direction)
        {
            // Get the currently selected button and find the next button in the list
            Button[] buttons = FindObjectsOfType<Button>();
            int currentIndex = System.Array.IndexOf(buttons, EventSystem.current.currentSelectedGameObject?.GetComponent<Button>());

            if (currentIndex == -1) return; // If no button is selected, return

            // Determine the next button index based on the direction (up or down arrow)
            int nextIndex = currentIndex + direction;
            nextIndex = Mathf.Clamp(nextIndex, 0, buttons.Length - 1); // Ensure we stay within bounds

            // Set the new selected button
            EventSystem.current.SetSelectedGameObject(buttons[nextIndex].gameObject);
        }


        public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Nivel2()
    {
        SceneManager.LoadScene(2);
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
        else
        {
            Debug.LogWarning("No more levels to load!");
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
