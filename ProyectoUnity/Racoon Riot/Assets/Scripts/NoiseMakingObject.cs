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
}
