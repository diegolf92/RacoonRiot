using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class MenuPrincipal : MonoBehaviour
{
    public Image hoverImage;  // Imagen que se mostrará al pasar el mouse
    public Vector3 offset;    // Offset para la posición de la imagen respecto al botón
    public AudioSource hoverSound;  // Sonido al seleccionar un botón con teclado
    public AudioSource clickSound;  // Sonido al confirmar selección con teclado
    public EventSystem eventSystem;
    public GameObject playButton;
    [SerializeField] private AudioMixer audioMixer;
    public GameObject PanelMenuPrincipal;
    public GameObject PanelOpciones;
    public GameObject PanelControles;
    public Slider slider;
    public GameObject firstSelectMainMenu;
    public GameObject firstSelectOption;
    public GameObject firstSelectControls;

    private GameObject lastSelectedButton; // Para detectar cambios en el botón seleccionado

    void Start()
    {
        hoverImage.gameObject.SetActive(false);

        // Inicializar el botón seleccionado al inicio
        if (playButton != null)
        {
            eventSystem.SetSelectedGameObject(playButton);
            lastSelectedButton = playButton;
        }
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject( eventSystem.firstSelectedGameObject );
        }

        if (eventSystem.currentSelectedGameObject != null)
        {
            // Verificar si el botón seleccionado cambió
            if (eventSystem.currentSelectedGameObject != lastSelectedButton)
            {
                // Actualizar la imagen de hover
                HighLight(eventSystem.currentSelectedGameObject);

                // Reproducir sonido al cambiar de selección
                if (hoverSound != null)
                {
                    hoverSound.Play();
                }

                // Actualizar el último botón seleccionado
                lastSelectedButton = eventSystem.currentSelectedGameObject;
            }

            // Confirmar selección al presionar la tecla Enter o Espacio
            if (Input.GetButtonDown("Submit"))
            {
                OnButtonClick();
            }
        }
    }

    public void OpenOptionsMenu()
    {
        eventSystem.SetSelectedGameObject(firstSelectOption);
        eventSystem.firstSelectedGameObject = firstSelectOption;
        PanelMenuPrincipal.SetActive(false);  // Ocultar el menú principal
        PanelOpciones.SetActive(true);        // Mostrar el menú de opciones
    }

    public void CloseOptionsMenu()
    {
        eventSystem.SetSelectedGameObject(firstSelectMainMenu);
        eventSystem.firstSelectedGameObject = firstSelectMainMenu;
        PanelOpciones.SetActive(false); // Ocultar el menú de opciones
        PanelMenuPrincipal.SetActive(true); // Mostrar el menú principal
    }

    public void OpenControlsMenu()
    {
        eventSystem.SetSelectedGameObject(firstSelectControls);
        eventSystem.firstSelectedGameObject = firstSelectControls;
        PanelControles.SetActive(true); // Mostrar el menú principal
        PanelOpciones.SetActive(false); // Ocultar el menú de opciones
        
    }

    public void CloseControlsMenu()
    {
        eventSystem.SetSelectedGameObject(firstSelectOption);
        eventSystem.firstSelectedGameObject = firstSelectOption;
        PanelOpciones.SetActive(true); // Ocultar el menú de opciones
        PanelControles.SetActive(false); // Mostrar el menú principal
    }

    public void jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void salir()
    {
        Application.Quit();
    }

    private void HighLight(GameObject target)
    {
        hoverImage.gameObject.SetActive(true);

        // Obtener RectTransform del botón y de la imagen
        RectTransform buttonRect = target.GetComponent<RectTransform>();
        RectTransform hoverRect = hoverImage.GetComponent<RectTransform>();

        // Posicionar la imagen hover cerca del botón
        hoverRect.position = buttonRect.position + offset;
    }

    private void OnPointerExit()
    {
        hoverImage.gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        // Reproducir sonido al confirmar la selección
        if (clickSound != null) clickSound.Play();
    }

    public void cambiarVolumen()
    {
        audioMixer.SetFloat("Volumen", slider.value);
    }
}
