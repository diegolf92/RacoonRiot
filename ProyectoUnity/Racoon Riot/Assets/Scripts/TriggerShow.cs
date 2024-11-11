using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShow : MonoBehaviour
{
    public GameObject sprite;

    private void Start()
    {
        sprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.SetActive(false);
        }
    }
}