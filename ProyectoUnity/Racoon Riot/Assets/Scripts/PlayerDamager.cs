using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamager : MonoBehaviour
{
    public CheckPointManager checkpoint;

    Animator anim;

    [Header("Vidas del Jugador")]
    public int vidas = 3;  // Número inicial de vidas
    public List<Image> vidaImages; // Lista de imágenes que representan las vidas

    [Header("Sonido de Muerte")]
    public AudioSource audioSource;  // Fuente de audio para reproducir el sonido
    public AudioClip deathSound;     // Clip de sonido de muerte

    [Header("Menú de Muerte")]
    public GameObject deathMenu;  // Referencia al menú de muerte

    [Header("Barra de Tiempo")]
    public Slider timeSlider;  // Slider que representa la barra de tiempo
    public float maxTime = 30f; // Tiempo máximo ajustable en el editor
    private float currentTime; // Tiempo actual

    void Start()
    {
        currentTime = maxTime; // Inicializar el tiempo
        if (timeSlider != null)
        {
            timeSlider.maxValue = maxTime; // Ajustar el valor máximo del slider
            timeSlider.value = currentTime; // Inicializar el valor del slider
        }
    }

    void Update()
    {
        if (vidas > 0) // Asegurarse de que solo se siga reduciendo el tiempo si hay vidas restantes
        {
            currentTime -= Time.deltaTime; // Reducir el tiempo actual

            if (timeSlider != null)
            {
                timeSlider.value = currentTime; // Actualizar el slider
            }

            if (currentTime <= 0)
            {
                ApplyDamage(); // Aplicar daño al jugador
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.activeInHierarchy)
        {
            ApplyDamage();
        }
    }

    void ApplyDamage()
    {
        vidas--;  // Restar una vida

        // Reproducir sonido de muerte
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Actualizar imágenes de vida
        if (vidaImages.Count > vidas && vidas >= 0)
        {
            vidaImages[vidas].enabled = false; // Desactivar la imagen correspondiente
        }

        // Si el jugador aún tiene vidas, reiniciar el tiempo
        if (vidas > 0)
        {
            checkpoint.Reviver();
            currentTime = maxTime; // Reiniciar el tiempo
            if (timeSlider != null)
            {
                timeSlider.value = currentTime; // Actualizar el slider
            }
        }
        else
        {
            HandlePlayerDeath();
        }
    }

    void HandlePlayerDeath()
    {
        Debug.Log("Game Over");
        gameObject.SetActive(false); // Desactivar el jugador.

        
        // Detener el juego
        Time.timeScale = 0f;

        // Activar el menú de muerte
        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
        }
    }

    public void AddTime(float extraTime)
    {
        currentTime += extraTime; // Añadir tiempo adicional
        if (timeSlider != null)
        {
            timeSlider.value = currentTime; // Actualizar el slider
        }
    }
}
