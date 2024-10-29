using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource sfxAudioSource;  // Fuente de audio para efectos de sonido

    [Header("Clips de Sonido")]
    public AudioClip itemCollectSound;
    public AudioClip buttonClickSound;
    public AudioClip doorOpenSound;
    public AudioClip leverSwitchSound;
    public AudioClip elevatorSound;
    // Agrega otros clips según los objetos interactuables que tengas en la escena

    private void Awake()
    {
        // Configuración del singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  // Asegurar solo una instancia
        }
    }

    // Métodos para reproducir diferentes sonidos
    public void PlayItemCollectSound()
    {
        PlaySound(itemCollectSound);
    }

    public void PlayButtonClickSound()
    {
        PlaySound(buttonClickSound);
    }

    public void PlayDoorOpenSound()
    {
        PlaySound(doorOpenSound);
    }

    public void PlayLeverSwitchSound()
    {
        PlaySound(leverSwitchSound);
    }
    public void PlayElevatorSound()
    {
        PlaySound(elevatorSound);
    }

    // Método general para reproducir cualquier sonido
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Clip de sonido no asignado en SoundManager.");
        }
    }
}
