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
    public EventSystem eventSystem;
    public GameObject playButton;


    void Start()
    {
        hoverImage.gameObject.SetActive(false);

    }

    private void Update()
    {
        if(Input.GetAxis("Submit") > 0.1f)
        {
            eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }

        if (eventSystem.currentSelectedGameObject == null) //si el jugador perdio el boton activo
        {
            eventSystem.SetSelectedGameObject( playButton.gameObject);
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
