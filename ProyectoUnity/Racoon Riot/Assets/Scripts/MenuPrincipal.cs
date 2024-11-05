using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour
{
    public Image hoverImage;  // La imagen que se mostrará al pasar el mouse
    public Vector3 offset;    // Offset para la posición de la imagen respecto al botón
    public AudioSource hoverSound;  // Sonido al pasar sobre un botón
    public AudioSource clickSound;  // Sonido al hacer clic en un botón
    private GameObject lastSelected;

    // Array para referenciar los botones del menú
    public Button[] botones;

    void Start()
    {
        hoverImage.gameObject.SetActive(false);

        foreach (Button boton in botones)
        {
            EventTrigger trigger = boton.gameObject.AddComponent<EventTrigger>();

            // PointerEnter
            EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
            pointerEnter.eventID = EventTriggerType.PointerEnter;
            pointerEnter.callback.AddListener((data) => { OnPointerEnter(boton); });
            trigger.triggers.Add(pointerEnter);

            // PointerExit
            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((data) => { OnPointerExit(); });
            trigger.triggers.Add(pointerExit);

            // OnClick
            boton.onClick.AddListener(() => OnButtonClick());
        }
    }

    private void Update()
    {
        // Get the currently selected GameObject
        GameObject currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        // Check if the selected object is different from the last frame
        if (currentSelected != lastSelected)
        {
            // If a button is selected, simulate hover behavior
            Button selectedButton = currentSelected?.GetComponent<Button>();
            if (selectedButton != null)
            {
                OnPointerEnter(selectedButton);
                lastSelected = currentSelected;
            }
        }

        // Detect "Space" or "Enter" key press for button click
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Button selectedButton = currentSelected?.GetComponent<Button>();
            if (selectedButton != null)
            {
                OnButtonClick();
                selectedButton.onClick.Invoke(); // Trigger the button click
            }
        }
    }
    public void jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void salir()
    {
        Debug.Log("saliendo del juego");
        Application.Quit();
    }

    private void OnPointerEnter(Button boton)
    {
        hoverImage.gameObject.SetActive(true);

        // Get the RectTransforms of the button and hoverImage
        RectTransform buttonRect = boton.GetComponent<RectTransform>();
        RectTransform hoverRect = hoverImage.GetComponent<RectTransform>();

        // Set the hoverImage position relative to the button position plus the offset
        hoverRect.position = buttonRect.position + offset;

        // Play the hover sound if set
        if (hoverSound != null) hoverSound.Play();
    }


        private void OnPointerExit()
    {
        hoverImage.gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        // Reproduce el sonido al hacer clic en el botón
        if (clickSound != null) clickSound.Play();
    }
}
