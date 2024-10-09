using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int roomPlayerIsOn = 0;
    public GameObject[] enemies;

    void Update()
    {
        switch (roomPlayerIsOn)
        {
            case 3:
                enemies[0].SetActive(true);
                enemies[1].SetActive(false);
                enemies[2].SetActive(false);
                enemies[3].SetActive(false);
                break;
            case 4:
                enemies[0].SetActive(false);
                enemies[1].SetActive(true);
                enemies[2].SetActive(false);
                enemies[3].SetActive(false);
                break;
            case 5:
                enemies[0].SetActive(false);
                enemies[1].SetActive(false);
                enemies[2].SetActive(true);
                enemies[3].SetActive(false);
                break;
            case 6:
                enemies[0].SetActive(false);
                enemies[1].SetActive(false);
                enemies[2].SetActive(false);
                enemies[3].SetActive(true);
                break;
            default:
                enemies[0].SetActive(false);
                enemies[1].SetActive(false);
                enemies[2].SetActive(false);
                enemies[3].SetActive(false);
                break;
        }
    }
}
