using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public List<Item> keyItems; // Lista de ítems clave requeridos para ganar
    private Player_Inventory playerInventory;
    public GameObject victoryMenu; // Referencia al menú de victoria
    PlayerController playerController;
    public bool canWin;
    public GameObject win_text;
    [Header("Gestión de Música")]
    public LevelMusicManager musicManager; // Referencia al LevelMusicManager

    void Start()
    {
        playerInventory = GetComponent<Player_Inventory>();
        victoryMenu.SetActive(false); // Asegurarse de que el menú esté desactivado al inicio
        win_text.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canWin)
        {
            playerController = other.GetComponent<PlayerController>();
            StartCoroutine(Victoria());
        }
        else if(other.CompareTag("Player") && !canWin)
        {
            Debug.Log("Aún no has recogido todos los objetos clave. Sigue buscando.");
        }
    }

    void CheckVictoryCondition()
    {
        bool hasAllKeyItems = true;
        foreach (Item keyItem in keyItems)
        {
            if (!playerInventory.inventory.items.Exists(i => i.item == keyItem))
            {
                hasAllKeyItems = false;
                break;
            }
        }

        if (hasAllKeyItems)
        {
            StartCoroutine(Victoria());
        }
        else
        {
            Debug.Log("Aún no has recogido todos los objetos clave. Sigue buscando.");
        }
    }

    IEnumerator Victoria()
    {
        // Detener la música de nivel
        if (musicManager != null)
        {
            musicManager.StopMusic();
        }

        // Reproducir música de victoria desde el SoundManager
        SoundManager.Instance?.PlayVictoryMusic();

        // Activar la animación
        playerController.GetComponent<PlayerController>().anim.SetBool("washing", true);

        // Mostrar texto de victoria
        win_text.SetActive(true);

        // Esperar 3 segundos para completar la animación
        yield return new WaitForSeconds(3f);

        // Detener la animación
        playerController.GetComponent<PlayerController>().anim.SetBool("washing", false);

        // Mostrar el menú de victoria
        victoryMenu.SetActive(true);

        // Opcional: Pausar el juego
        // Time.timeScale = 0f;
    }
}
