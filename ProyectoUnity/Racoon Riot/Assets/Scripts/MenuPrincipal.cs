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

    // Array para referenciar los botones del menú
    public Button[] botones;

    void Start()
    {
        // Asegúrate de que la imagen esté desactivada al inicio
        hoverImage.gameObject.SetActive(false);

        // Asignar los métodos de los eventos a los botones
        foreach (Button boton in botones)
        {
            // Agregar listeners para los eventos de pointer enter y exit
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

    // Método que se llamará cuando el mouse entre en el botón
    private void OnPointerEnter(Button boton)
    {
        hoverImage.gameObject.SetActive(true);  // Activar la imagen
        hoverImage.transform.position = boton.transform.position + offset;  // Colocar la imagen con el offset
    }

    // Método que se llamará cuando el mouse salga del botón
    private void OnPointerExit()
    {
        hoverImage.gameObject.SetActive(false);  // Desactivar la imagen
    }
}
