using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethSc : MonoBehaviour
{
    public float speed = 1f;
    public Transform target;
    Transform originalPos;
    public bool specialOn;
    bool goBack;

    private void Start()
    {
        originalPos = transform;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        StartCoroutine(TargetLocation());
    }

    private void Update()
    {
        if(specialOn)transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if(goBack)transform.position = Vector3.MoveTowards(transform.position, originalPos.position, speed * Time.deltaTime);
    }

    IEnumerator TargetLocation()
    {
        specialOn = true;

        yield return new WaitForSeconds(2f);

        specialOn = false;

        StartCoroutine(GoBackToSpot());
    }

    IEnumerator GoBackToSpot()
    {
        goBack = true;
        yield return new WaitForSeconds(2.1f);
        goBack = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLife playerDamager = other.GetComponent<PlayerLife>();
            if (playerDamager != null)
            {
                playerDamager.EnemyDamage();
            }
        }
    }
}
