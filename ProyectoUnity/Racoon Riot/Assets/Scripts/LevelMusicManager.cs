using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelMusicManager : MonoBehaviour
{
    [Header("Música de Fondo")]
    public AudioSource levelMusicSource;  // AudioSource para la música de fondo
    public AudioClip levelMusic;          // Música estándar del nivel

    [Header("Música de Advertencia")]
    public AudioClip warningMusic;        // Música de advertencia cuando el tiempo está por acabarse
    public float warningThreshold = 10f;  // Tiempo en segundos para activar la música de advertencia

    [Header("Referencia al Slider de Tiempo")]
    public Slider timeSlider;             // Referencia al Slider del tiempo

    private bool warningMusicPlayed = false;

    void Start()
    {
        // Asegurarse de que la música estándar suene al inicio
        levelMusicSource.clip = levelMusic;
        levelMusicSource.loop = true;
        levelMusicSource.Play();
    }

    void Update()
    {
        // Verificar si el tiempo en el slider está cerca del límite
        if (timeSlider != null && timeSlider.value <= warningThreshold && !warningMusicPlayed)
        {
            // Cambiar la música a la de advertencia
            PlayWarningMusic();
        }
    }

    void PlayWarningMusic()
    {
        levelMusicSource.Stop();
        levelMusicSource.clip = warningMusic;
        levelMusicSource.loop = true;
        levelMusicSource.Play();
        warningMusicPlayed = true;
    }

    public void ResetMusic()
    {
        // Restablece la música estándar si es necesario (por ejemplo, si el tiempo se reinicia)
        warningMusicPlayed = false;
        levelMusicSource.Stop();
        levelMusicSource.clip = levelMusic;
        levelMusicSource.Play();
    }
    public void StopMusic()
    {
        levelMusicSource.Stop();
    }
}
    