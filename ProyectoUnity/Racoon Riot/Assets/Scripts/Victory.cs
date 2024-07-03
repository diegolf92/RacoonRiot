using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    public List<Item> keyItems; // Lista de ítems clave requeridos para ganar
    private Player_Inventory playerInventory;

    void Start()
    {
        playerInventory = GetComponent<Player_Inventory>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si el jugador llega al punto de meta
        if (other.CompareTag("Goal"))
        {
            CheckVictoryCondition();
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
            Debug.Log("Aún no has recogido todos los objetos clave. Sigue buscando.");
        }
    }

    void Victoria()
    {
        // Aquí puedes implementar la lógica de victoria, como cargar una nueva escena o mostrar un mensaje de victoria
        Debug.Log("¡Victoria! Has recogido todos los objetos clave y has llegado a la meta.");
    }
}
