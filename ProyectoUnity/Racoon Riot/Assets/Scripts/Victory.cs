using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public List<Item> keyItems; // Lista de �tems clave requeridos para ganar
    private Player_Inventory playerInventory;
    public GameObject victoryMenu; // Referencia al men� de victoria

    void Start()
    {
        playerInventory = GetComponent<Player_Inventory>();
        victoryMenu.SetActive(false); // Asegurarse de que el men� est� desactivado al inicio
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si el jugador llega al punto de meta
        if (other.CompareTag("Goal"))
        {
            CheckVictoryCondition();
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().anim.SetBool("muerteHambre", true);
            StartCoroutine(Victoria());
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
            Victoria();
        }
        else
        {
            Debug.Log("A�n no has recogido todos los objetos clave. Sigue buscando.");
        }
    }

    IEnumerator Victoria()
    {
        yield return new WaitForSeconds(2f);
        // Mostrar el men� de victoria
        victoryMenu.SetActive(true);
        // Pausar el juego si es necesario
        Time.timeScale = 0f;
    }
}
