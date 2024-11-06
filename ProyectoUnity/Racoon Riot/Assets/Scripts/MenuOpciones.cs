using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuOpciones : MonoBehaviour

{
    [SerializeField]private AudioMixer audioMixer;
    [SerializeField] private GameObject mainMenu;    // Reference to the Main Menu
    [SerializeField] private GameObject optionsMenu; // Reference to the Options Menu
    [SerializeField] private Button[] buttons;

    private Button currentButton;  // Current selected button
    private GameObject lastSelected; // Track the last selected button

    void Start()
    {
        // Initially set the main menu to active and options menu to inactive
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);

        // Set the initial button selection in the options menu
        if (buttons.Length > 0)
        {
            currentButton = buttons[0];
            EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
        }
    }

    void Update()
    {
        // Handle Arrow Key navigation
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Navigate(-1); // Navigate up
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Navigate(1);  // Navigate down
        }

        // Handle Space or Enter key press to activate the selected button
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (currentButton != null)
            {
                currentButton.onClick.Invoke(); // Trigger the button click
            }
        }
    }

    // Function to navigate through buttons
    private void Navigate(int direction)
    {
        int currentIndex = System.Array.IndexOf(buttons, currentButton);
        int newIndex = Mathf.Clamp(currentIndex + direction, 0, buttons.Length - 1);

        // Set the new selected button
        currentButton = buttons[newIndex];
        EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
    }

    // Function to change volume
    public void cambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
    }

    // Function to open the options menu and close the main menu
    public void OpenOptionsMenu()
    {
        mainMenu.SetActive(false);  // Hide the main menu
        optionsMenu.SetActive(true); // Show the options menu
    }

    // Function to go back to the main menu from options menu
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false); // Hide the options menu
        mainMenu.SetActive(true);    // Show the main menu
    }
}

  

