using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemy : MonoBehaviour
{
    public float maxDistance = 5f;
    public GameObject[] enemies;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, this.transform.position) > maxDistance)
            {
                enemies[i].SetActive(false);
            }
            else
            {
                enemies[i].SetActive(true);
            }
        }
    }
}
