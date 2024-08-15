using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMakingObject : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 2f;
    public KeyCode interactionKey = KeyCode.E;
    public AudioClip noiseClip;
    public float noiseRadius = 10f; // How far the noise will be heard
    private AudioSource audioSource;
    public float cooldownTime = 2f;
    private bool isOnCooldown = false;
    public EnemyAi enemyScript;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = noiseClip;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (Vector2.Distance(player.position, transform.position) < interactionDistance && Input.GetKeyDown(interactionKey) && !isOnCooldown)
        {
            MakeNoise();
        }
    }

    void MakeNoise()
    {
        audioSource.Play();
        if (enemyScript.currentState == EnemyAi.EnemyState.VIGILANDO) 
        {
            enemyScript.DistractEnemy(transform);
            enemyScript.ChangeEnemyState(2, gameObject);
        } 
        StartCoroutine(Cooldown());
        //AlertEnemies();
    }

    private IEnumerator Cooldown()
    {
        isOnCooldown = true; // Set the object on cooldown
        yield return new WaitForSeconds(cooldownTime); // Wait for the cooldown time
        isOnCooldown = false; // Reset the cooldown flag
    }

    /*
    void AlertEnemies()
    {
        // Find all enemies within the noise radius and alert them
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, noiseRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                EnemyAI enemy = hitCollider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.SetTargetPosition(transform.position);
                }
            }
        }
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            MakeNoise();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            if (enemyScript.currentState == EnemyAi.EnemyState.REVISANDO)
            {
                enemyScript.objectOnSight = null;
                enemyScript.ChangeEnemyState(4);
            }
        }
    }
}
