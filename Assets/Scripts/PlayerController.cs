using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Public variables
    [Header("Player Settings")]
    [Range(0, 100)] public float health;
    public int spiritsPickedUp;

    [Header("Movement Settings")]
    [Range(0, 15)] public float moveSpeed;
    [Range(100, 750)] public float jumpForce;

    [Header("Magic Settings")]
    [Range(0, 5)] public float magicDelay;

    [Header("Other Settings")]
    public bool isInMenu;
    public bool isDead;

    [Header("Referencing Note Objects")]
    public Canvas noteCanvas;
    public Text noteText;

    [Header("Referencing GameObjects")]
    public Animator animatorController;
    public GameObject projectile;
    public Transform projectileSpawn;
    public HUDController hudController;
    public CodelockPuzzle puzzleController;

    // Private variables
    private bool isGrounded;
    private bool isMovingleft;
    private float nextMagic;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Called once per frame
    void Start()
    {
        // Fetch the RigidBody2D
        rb = GetComponent<Rigidbody2D>();

        // Fetch the SpriteRenderer
        sr = GetComponent<SpriteRenderer>();

        // Disable the note canvas
        noteCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the players input
        float moveX = Input.GetAxis("Horizontal");

        // Move the player based on the input
        if(!isInMenu && !isDead)
        {
            transform.Translate(Vector2.right * Time.deltaTime * moveSpeed * moveX);
        }

        // Check if the player is dead
        if(health == 0)
        {
            StartCoroutine("PlayDeath");
        }

        // Check if the player is moving left for anims
        if (moveX < 0 && !isInMenu)
        {
            // Flip sprite
            sr.flipX = true;

            // Set to true
            isMovingleft = true;

            // Rotate ProjectileSpawn
            projectileSpawn.position = transform.position + new Vector3(-1.6f, 0, 0);
        } else
        {
            // Reverse the flip sprite
            sr.flipX = false;
            
            // Set to false
            isMovingleft = false;

            // Rotate ProjectileSpawn
            projectileSpawn.position = transform.position + new Vector3(1.6f, 0, 0);
        }

        // Check if the player is moving for anims
        if(moveX > 0 && !isInMenu || moveX < 0 && !isInMenu)
        {
            animatorController.SetBool("isRunning", true);
        } else
        {
            animatorController.SetBool("isRunning", false);
        }

        // Check if the player is on the ground
        if(Input.GetButtonDown("Jump") && isGrounded && !isInMenu && !isDead)
        {
            // Apply the force to the player
            rb.AddForce(Vector2.up * jumpForce);

            // Set the jumping animation
            animatorController.SetBool("isJumping", true);

            // Prevent the player from jumping again
            isGrounded = false;
        }

        // Fire magic
        if(Input.GetButton("Fire1") && Time.time > nextMagic && !isInMenu && !isDead)
        {
            // Set the next magic time
            nextMagic = Time.time + magicDelay;

            // Store projectile
            GameObject projectileToSpawn = projectile;

            // Set the fire direction
            if(isMovingleft == true)
            {
                projectileToSpawn.GetComponent<Projectile>().directionToFire = Projectile.FireDirections.Left;
            } else
            {
                projectileToSpawn.GetComponent<Projectile>().directionToFire = Projectile.FireDirections.Right;
            }

            // Spawn projectile
            Instantiate(projectileToSpawn, projectileSpawn.position, Quaternion.identity);
        }
    }

    // Health function
    public void updateHealth(int healthToUpdate)
    {
        // Set health
        health += healthToUpdate;

        // Upate UI
        hudController.UpdateHealthUI(health);
    }

    // When the player collides
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Switch through the different tags
        switch(collision.gameObject.tag)
        {
            // Ground
            case "Ground":

                // Set player on the ground
                isGrounded = true;

                // Reset animation
                animatorController.SetBool("isJumping", false);

                break;

            // Pickup
            case "Pickup":

                // Destroy the pickup
                Destroy(collision.gameObject);

                // Code for pickups
                spiritsPickedUp += 1;

                hudController.UpdateSpiritsUI(spiritsPickedUp);

                break;
        }
    }

    // Display the note for 3 seconds then disappear
    IEnumerator TimedNote()
    {
        noteCanvas.enabled = true;

        yield return new WaitForSeconds(3);

        noteCanvas.enabled = false;
    }

    // Death system
    IEnumerator PlayDeath()
    {
        // Stop movement
        isDead = true;

        // Set animation
        animatorController.SetBool("isDead", true);

        yield return new WaitForSeconds(2);

        // Send to main menu
        SceneManager.LoadScene(0);
    }

    IEnumerator OpenBox()
    {
        // Open box


        // Wait for 3 seconds
        yield return new WaitForSeconds(3);

        // Load credits scene
        SceneManager.LoadScene(2);
    }

    // If the player stays in the door
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the trigger is interactable
        if (collision.gameObject.tag == "Interactable")
        {
            // Check if player interacts
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Get the interactable type
                Interactable.interactionTypes interactionType = collision.gameObject.GetComponent<Interactable>().interactionType;

                // Switch between the interaction types
                switch (interactionType)
                {
                    // Codelock Puzzle
                    case Interactable.interactionTypes.Codelock:
                        // Display the code lock
                        puzzleController.DisplayCodelock();
                        break;

                    // Note
                    case Interactable.interactionTypes.Note:
                        // Get note component
                        Note note = collision.gameObject.GetComponent<Note>();

                        // Display note on screen
                        noteText.text = "" + note.numberToDisplay;

                        StartCoroutine("TimedNote");

                        // Delete note
                        Destroy(collision.gameObject);
                        
                        break;

                    case Interactable.interactionTypes.Box:

                        // Check if player has got all of the spirits
                        if(spiritsPickedUp == 4)
                        {
                            StartCoroutine("OpenBox");
                        }

                        break;
                }
            }
        }
    }
}
