using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject roomCam;
    public int roomNumber;
    GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)   
        {
            roomCam.SetActive(true);
            gm.roomPlayerIsOn = roomNumber;
        }     
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)   
        {
            roomCam.SetActive(false);
        }     
    }
}
