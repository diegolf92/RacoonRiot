using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHazard : MonoBehaviour
{

    public float activeDuration = 2f;  // Time laser stays active
    public float inactiveDuration = 2f;  // Time laser stays inactive
    public float startDelay = 0f;  // Custom delay for each laser
    private bool isActive = false;  // Whether the laser is currently active
    private Collider2D laserCollider;  // Collider of the laser
    public SpriteRenderer laserSprite;  // Visual representation of the laser

    private void Start()
    {
        laserCollider = GetComponent<Collider2D>();
        StartCoroutine(ToggleLaser(startDelay));  // Start the laser with a delay
    }

    private IEnumerator ToggleLaser(float delay)
    {
        // Initial delay to stagger the laser start times
        yield return new WaitForSeconds(delay);

        while (true)  // Infinite loop for laser activation and deactivation
        {
            // Turn laser on
            isActive = true;
            laserCollider.enabled = true;
            laserSprite.enabled = true;  // Show laser visually
            yield return new WaitForSeconds(activeDuration);

            // Turn laser off
            isActive = false;
            laserCollider.enabled = false;
            laserSprite.enabled = false;  // Hide laser visually
            yield return new WaitForSeconds(inactiveDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            PlayerDamager playerDamager = other.GetComponent<PlayerDamager>();
            if (playerDamager != null)
            {
                playerDamager.ApplyDamage();  // Apply damage to the player when laser is active
            }
        }
    }
}