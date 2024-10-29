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
        hoverImage.transform.position = boton.transform.position + offset;

        // Reproduce el sonido al pasar el mouse sobre el botón
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
