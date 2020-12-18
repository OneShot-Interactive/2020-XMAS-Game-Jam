using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Public variables
    public enum FireDirections { Left, Right }

    [Header("Projectile Settings")]
    [Range(0, 3000)] public float projectileForce;
    [Range(0, 10)] public float projectileLifetime;
    public FireDirections directionToFire;

    // Private variables
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float currentLifetime;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch RigidBody2D
        rb = GetComponent<Rigidbody2D>();

        // Fetch Sprite Render
        sr = GetComponent<SpriteRenderer>();

        // Add force based on direction
        if(directionToFire == FireDirections.Left)
        {
            rb.AddForce(Vector2.left * projectileForce);
            sr.flipX = true;
        } else
        {
            rb.AddForce(Vector2.right * projectileForce);
            sr.flipX = false;
        }
    }

    // Called once per frame
    void Update()
    {
        // Update time
        currentLifetime += Time.deltaTime;

        // Check if the projectile isn't alive for too long
        if(currentLifetime >= projectileLifetime)
        {
            Destroy(gameObject);
        }
    }

    // On trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If trigger was an enemy
        if (collision.gameObject.tag == "Enemy") {
            // Set the AI to dead
            collision.gameObject.GetComponent<AIController>().currentAiState = AIController.AIStates.Dead;

            // Delete Projectile
            Destroy(gameObject);
        }
    }
}
