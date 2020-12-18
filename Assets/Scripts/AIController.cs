using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Public variables
    public enum AIStates
    {
        Idle,
        Patrolling,
        Attacking,
        Dead
    }

    [Header("AI Settings")]
    public int aiHealth;
    [Range(0, 10)] public float aiMoveSpeed;
    public AIStates currentAiState;

    [Header("AI State Settings")]
    [Range(0, 20)] public float viewDistance;
    [Range(0, 10)] public float idleDuration;
    [Range(0, 10)] public float patrolMinDuration;
    [Range(10, 30)] public float patrolMaxDuration;
    [Range(0, 30)] public float deathLifetime;

    [Header("AI Attack Settings")]
    [Range(0, 10)] public float aiAttackRange;
    [Range(0, 15)] public float aiAttackDelay;

    [Header("AI Animation Settings")]
    public Animator aiAnimator;

    [Header("Referencing GameObjects")]
    public GameObject playerReward;
    public Transform groundRayPoint;
    public Transform playerPos;
    public PlayerController playerController;

    // Private variables
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    private bool isMovingRight;
    private bool isRewardSpawned;
    private float timeSinceIdle, timeSincePatrol, timeSinceDeath;
    private float nextAttack;
    private float playerDist;
    private float patrolDuration;

    // Start is called before the first frame update
    void Start()
    {
        // Reset the animation parameters
        aiAnimator.SetBool("isWalking", false);
        aiAnimator.SetBool("isAttacking", false);
        aiAnimator.SetBool("isDead", false);

        // Set initial state
        currentAiState = AIStates.Idle;

        // Set movingRight
        isMovingRight = true;

        // Set patrol duration
        patrolDuration = Random.Range(patrolMinDuration, patrolMaxDuration);

        // Fetch RigidBody2D
        rb = GetComponent<Rigidbody2D>();

        // Fetch BoxCollider2D
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the distance to player
        playerDist = Vector2.Distance(transform.position, playerPos.position);

        // Check the ai health
        if(aiHealth == 0)
        {
            // Set the AI to the dead state
            currentAiState = AIStates.Dead;
        }

        // What state the AI should be in
        switch(currentAiState)
        {
            // Idle
            case AIStates.Idle:

                // Reset animations states
                aiAnimator.SetBool("isWalking", false);
                aiAnimator.SetBool("isAttacking", false);
                aiAnimator.SetBool("isDead", false);

                // Set idle timer
                timeSinceIdle += Time.deltaTime;

                // Check if timer is equal or greater then duration
                if (timeSinceIdle >= idleDuration)
                {
                    // Change to patrolling state
                    currentAiState = AIStates.Patrolling;

                    // Reset timeSinceIdle
                    timeSinceIdle = 0;
                }

                // Check if player is in range
                if(playerDist < aiAttackRange)
                {
                    currentAiState = AIStates.Attacking;
                }

                break;

            // Patrolling
            case AIStates.Patrolling:

                // Reset animation states
                aiAnimator.SetBool("isAttacking", false);

                // Move the AI right
                transform.Translate(Vector2.right * Time.deltaTime * aiMoveSpeed);

                // Set patrol timer
                timeSincePatrol += Time.deltaTime;

                // Check if timer is equal or greater then duration
                if(timeSincePatrol >= patrolDuration)
                {
                    // Stop animation
                    aiAnimator.SetBool("isWalking", false);

                    // Change to idle state
                    currentAiState = AIStates.Idle;

                    // Reset timer
                    timeSincePatrol = 0;
                }

                // Set the isWalking bool
                aiAnimator.SetBool("isWalking", true);

                // Send a raycast into the ground
                RaycastHit2D groundInfo = Physics2D.Raycast(groundRayPoint.position, Vector2.down);

                // If the AI does not detect ground
                if(groundInfo.collider.gameObject.tag != "Ground")
                {
                    // Change the rotation based on movement direction
                    if(isMovingRight)
                    {
                        // Rotate the player
                        transform.eulerAngles = new Vector3(0, -180, 0);

                        // Set to moving left
                        isMovingRight = false;
                    } else
                    {
                        // Rotate the player
                        transform.eulerAngles = new Vector3(0, 0, 0);

                        // Set to moving right
                        isMovingRight = true;
                    }
                }

                // Check if the player is in range for attack
                if(playerDist < aiAttackRange)
                {
                    currentAiState = AIStates.Attacking;
                }

                break;

            // Attacking 
            case AIStates.Attacking:

                // Reset animation states
                aiAnimator.SetBool("isWalking", false);

                // Check if the player has escaped
                if(playerDist > aiAttackRange)
                {
                    currentAiState = AIStates.Patrolling;
                }

                // Set AI to look at player
                if(transform.position.x > playerPos.position.x)
                {
                    // Set AI to the left
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    isMovingRight = false;
                } else
                {
                    // Set AI to the right
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    isMovingRight = false;
                }

                // Attack the player
                if(Time.time > nextAttack)
                {
                    // Set the next time to attack
                    nextAttack = Time.time + aiAttackDelay;

                    RaycastHit2D attackRay;

                    // See if player is in reach of spear based on direction of movement
                    if(isMovingRight)
                    {
                        attackRay = Physics2D.Raycast(transform.position, Vector2.right, aiAttackRange);
                        Debug.DrawRay(transform.position, Vector2.right * aiAttackRange, Color.red, 3);
                    }
                    else
                    {
                        attackRay = Physics2D.Raycast(transform.position, Vector2.left, aiAttackRange);
                        Debug.DrawRay(transform.position, Vector2.left * aiAttackRange, Color.red, 3);
                    }

                    // Set animation
                    aiAnimator.SetBool("isAttacking", true);

                    // Withdraw health from player
                    if(attackRay.collider.gameObject.tag == "Player")
                    { 
                        playerController.updateHealth(-25);
                    }
                }

                break;

            // Dead
            case AIStates.Dead:

                // Disable collider and rigidbody
                Destroy(rb);
                Destroy(bc2d);

                // Spawn christmas spirit above the dead AI
                if(!isRewardSpawned)
                {
                    Instantiate(playerReward, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                    isRewardSpawned = true;
                }

                // Reset animation states
                aiAnimator.SetBool("isWalking", false);
                aiAnimator.SetBool("isAttacking", false);

                // Set dead animation state
                aiAnimator.SetBool("isDead", true);

                // Start timer
                timeSinceDeath += Time.deltaTime;

                // Check if timer is equal or greater then lifetime
                if(timeSinceDeath >= deathLifetime)
                {
                    Destroy(gameObject);
                }

                break;
        }
    }
}
