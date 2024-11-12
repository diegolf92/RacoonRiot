using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
//using UnityEngine.UIElements;

public class MenuPrincipal : MonoBehaviour
{
    public Image hoverImage;  // La imagen que se mostrará al pasar el mouse
    public Vector3 offset;    // Offset para la posición de la imagen respecto al botón
    public AudioSource hoverSound;  // Sonido al pasar sobre un botón
    public AudioSource clickSound;  // Sonido al hacer clic en un botón
    public EventSystem eventSystem;
    public GameObject playButton;
    [SerializeField] private AudioMixer audioMixer;
    public GameObject PanelMenuPrincipal;
    public GameObject PanelOpciones;
    public Slider slider;

    //private bool libre;

    void Start()
    {
        //libre = true;
        hoverImage.gameObject.SetActive(false);

    }

    private void Update()
    {
        /*
        if(libre & Input.GetButtonUp("Submit"))
        {
            libre = false;
            Debug.Log(eventSystem.currentSelectedGameObject.name);
            eventSystem.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
        else
        {
            libre = true;
        }
        */

        if (eventSystem.currentSelectedGameObject != null ) //si el jugador perdio el boton activo
        {
            if(eventSystem.currentSelectedGameObject != null)
                HighLight(eventSystem.currentSelectedGameObject);
            //eventSystem.SetSelectedGameObject( playButton.gameObject);
        }
    }

    public void OpenOptionsMenu()
    {
        Debug.Log("OpenOptionsMenu");
        PanelMenuPrincipal.SetActive(false);  // Hide the main menu
        PanelOpciones.SetActive(true); // Show the options menu
    }

    // Function to go back to the main menu from options menu
    public void CloseOptionsMenu()
    {
        Debug.Log("CloseOptionsMenu");
        PanelOpciones.SetActive(false); // Hide the options menu
        PanelMenuPrincipal.SetActive(true);    // Show the main menu
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
        //if (hoverSound != null) hoverSound.Play();
    }

    private void HighLight(GameObject target)
    {
        hoverImage.gameObject.SetActive(true);

        // Get the RectTransforms of the button and hoverImage
        RectTransform buttonRect = target.GetComponent<RectTransform>();
        RectTransform hoverRect = hoverImage.GetComponent<RectTransform>();

        // Set the hoverImage position relative to the button position plus the offset
        hoverRect.position = buttonRect.position + offset;

        // Play the hover sound if set
        //if (hoverSound != null) hoverSound.Play();
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

    public void cambiarVolumen()
    {
        audioMixer.SetFloat("Volumen", slider.value);
    }
}
