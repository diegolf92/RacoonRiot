using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class PlayerLife : MonoBehaviour
{
    public CheckPointManager checkpoint;

    [Header("Vidas del Jugador")]
    public int vidas = 3;  // N�mero inicial de vidas
    public List<Image> vidaImages; // Lista de im�genes que representan las vidas

    [Header("Sonido de Muerte")]
    public AudioSource audioSource;  // Fuente de audio para reproducir el sonido
    public AudioClip deathSound;     // Clip de sonido de muerte

    [Header("Men� de Muerte")]
    public GameObject deathMenu;  // Referencia al men� de muerte

    [Header("Barra de Tiempo")]
    public Slider timeSlider;  // Slider que representa la barra de tiempo
    public float maxTime = 100f; // Tiempo m�ximo ajustable en el editor
    private float currentTime; // Tiempo actual
    public PlayerController player;
    bool isDead = false;

    void Start()
    {
        currentTime = maxTime; // Inicializar el tiempo
        if (timeSlider != null)
        {
            timeSlider.maxValue = maxTime; // Ajustar el valor m�ximo del slider
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
                if (!isDead)
                {
                    isDead = true;
                    StartCoroutine(HandlePlayerDeath()); // Aplicar da�o al jugador
                }
            }
        }
        else { }
    }

    public void ApplyDamage()
    {
        // Si el jugador a�n tiene vidas, reiniciar el tiempo
        if (vidas > 0)
        {
            vidas--;  // Restar una vida
                      // Actualizar im�genes de vida
            if (vidaImages.Count > vidas && vidas >= 0)
            {
                vidaImages[vidas].enabled = false; // Desactivar la imagen correspondiente
            }
            checkpoint.Reviver();
            currentTime = maxTime; // Reiniciar el tiempo
            if (timeSlider != null)
            {
                timeSlider.value = currentTime; // Actualizar el slider
            }
        }
        else if (vidas == 0){
            checkpoint.Reviver();
            isDead = true;
            StartCoroutine(HandlePlayerDeath()); // Aplicar da�o al jugador
        }
    }

    public void EnemyDamage()
    {
        // Si el jugador a�n tiene vidas, reiniciar el tiempo
        if (vidas > 0)
        {
            vidas--;  // Restar una vida
                      // Actualizar im�genes de vida
            if (vidaImages.Count > vidas && vidas >= 0)
            {
                vidaImages[vidas].enabled = false; // Desactivar la imagen correspondiente
            }
            currentTime = maxTime; // Reiniciar el tiempo
            if (timeSlider != null)
            {
                timeSlider.value = currentTime; // Actualizar el slider
            }
        }
        else if (vidas == 0)
        {
            isDead = true;
            if (deathMenu != null)
            {
                deathMenu.SetActive(true);
            }
        }
    }

    public void TimeSliderAdd()
    {
        if ((currentTime > maxTime))
        {
            currentTime += 25f;
        }
        else
        {
            currentTime = maxTime;
        }

    }

    IEnumerator HandlePlayerDeath()
    {
        //gameObject.SetActive(false); // Desactivar el jugador
        player.currentState = PlayerState.MURIENDO;
        //checkpoint.Reviver();
        yield return new WaitForSeconds(3f);

        if (deathMenu != null)
        {
            deathMenu.SetActive(true);
        }
    }
}
