using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethSc : MonoBehaviour
{
    public float speed = 2f;
    public Transform target;
    Transform originalPos;
    public bool specialOn;
    bool goBack;
    public EnemyAi parentEnemy;

    private void Start()
    {
        originalPos = transform;
    }

    private void Update()
    {
        if(specialOn)transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        if(goBack)transform.position = Vector3.MoveTowards(transform.position, this.transform.parent.position, speed * Time.deltaTime);
    }

    public void ChasePlayer()
    {
        StartCoroutine(TargetLocation());
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
        parentEnemy.ChangeEnemyState(4);
        parentEnemy.specialOn = false;
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
                parentEnemy.ChangeEnemyState(4);
                parentEnemy.specialOn = false;
            }
        }
    }
}
