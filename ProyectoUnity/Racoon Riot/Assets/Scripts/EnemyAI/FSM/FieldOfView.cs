using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float fovAngle = 45f;
    public int rayCount = 10;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public LayerMask triggerLayer;
    public bool playerDetected = false;
    public GameObject enemySprite;
    public GameObject enemyScripts;
    public bool isFacingRight = true;

    private void Update()
    {
        if (transform.position.x > enemySprite.transform.position.x)
        {
            isFacingRight = true;
        } else { isFacingRight = false; }
    }

    public void DetectLayers()
    {
        
        float angleStep = fovAngle / (rayCount - 1);
        float startAngle = -fovAngle / 2;

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 direction = AngleToDirection(currentAngle);
            if(!isFacingRight)direction *= -1;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius);
            Debug.Log(detectionRadius.ToString() + " ?? " + direction.ToString());

            if (hit.collider != null)
            {
                if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
                {
                    HandlePlayerDetection(hit.collider);
                }
                else if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
                {
                    HandleObstacleDetection(hit.collider);
                }
                else if (((1 << hit.collider.gameObject.layer) & triggerLayer) != 0)
                {
                    HandleTriggerDetection(hit.collider);
                }
            }

            Debug.DrawRay(transform.position, direction * detectionRadius, Color.grey);
        }
    }

    private Vector2 AngleToDirection(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    private void HandlePlayerDetection(Collider2D player)
    {
        playerDetected = true;
    }

    private void HandleObstacleDetection(Collider2D obstacle)
    {
    }

    private void HandleTriggerDetection(Collider2D trigger)
    {
        Debug.Log("Trigger detected!");
        // Add behavior for when a trigger is detected
    }

    public bool Flip()
    {
        // Flip the enemy's scale on the X-axis
        Vector3 scale = enemyScripts.transform.localScale;
        scale.x *= -1;
        enemyScripts.transform.localScale = scale;
        return true;
    }
}
