using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour
{
    public Image hoverImage;  // La imagen que se mostrar� al pasar el mouse
    public Vector3 offset;    // Offset para la posici�n de la imagen respecto al bot�n

    // Array para referenciar los botones del men�
    public Button[] botones;

    void Start()
    {
        // Aseg�rate de que la imagen est� desactivada al inicio
        hoverImage.gameObject.SetActive(false);

        // Asignar los m�todos de los eventos a los botones
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

    // M�todo que se llamar� cuando el mouse entre en el bot�n
    private void OnPointerEnter(Button boton)
    {
        hoverImage.gameObject.SetActive(true);  // Activar la imagen
        hoverImage.transform.position = boton.transform.position + offset;  // Colocar la imagen con el offset
    }

    // M�todo que se llamar� cuando el mouse salga del bot�n
    private void OnPointerExit()
    {
        hoverImage.gameObject.SetActive(false);  // Desactivar la imagen
    }
}
