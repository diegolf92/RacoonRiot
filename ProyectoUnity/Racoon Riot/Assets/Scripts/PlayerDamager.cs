using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    public CheckPointManager checkpoint;

    [Header("Vidas del Jugador")]
    public int vidas = 3;  // Número inicial de vidas

    [Header("Sonido de Muerte")]
    public AudioSource audioSource;  // Fuente de audio para reproducir el sonido
    public AudioClip deathSound;     // Clip de sonido de muerte

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.activeInHierarchy)
        {
            vidas--;  // Restar una vida

            // Reproducir sonido de muerte
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            // Revivir al jugador si aún le quedan vidas
            if (vidas > 0)
            {
                checkpoint.Reviver();
            }
            else
            {
                
                other.gameObject.SetActive(false); // Desactivar el jugador
                Debug.Log("Game Over");
                
            }
        }
    }
}
