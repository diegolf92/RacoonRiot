using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Content;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabOnRoom;
    public GameObject roomCam;
    //public int roomNumber;
    GameManager gm;
    bool playerIsOnThisRoom;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (playerIsOnThisRoom && enemyPrefabOnRoom != null)
        {
            foreach (var item in enemyPrefabOnRoom)
            {
                item.SetActive(true);
            }
        }
        else if(!playerIsOnThisRoom && enemyPrefabOnRoom != null)
        {
            foreach (var item in enemyPrefabOnRoom)
            {
                item.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)   
        {
            playerIsOnThisRoom = true;
            roomCam.SetActive(true);
            //gm.roomPlayerIsOn = roomNumber;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)   
        {
            playerIsOnThisRoom = false;
            roomCam.SetActive(false);
        }     
    }
}
