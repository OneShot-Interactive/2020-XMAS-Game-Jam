using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    // Public variables
    [Header("Referencing GameObjects")]
    public Canvas pauseMenu;
    public DiscordController discordController;

    // Private variables
    private bool isPaused;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Set to false
        isPaused = false;

        // Fetch PlayerController
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Hide the menu
        pauseMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player pressed pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                // Display the menu
                pauseMenu.enabled = true;

                // Set is in menu
                playerController.isInMenu = true;

                // Set the game to paused
                isPaused = true;

                // Freeze the game
                Time.timeScale = 0;
            } else
            {
                // Display the menu
                pauseMenu.enabled = false;

                // Set is in menu
                playerController.isInMenu = false;

                // Set the game to paused
                isPaused = false;

                // Freeze the game
                Time.timeScale = 1;
            }
        }
    }

    // Load scene
    public void LoadScene(int toLoad)
    {
        // Reset the timeScale
        Time.timeScale = 1;

        // Set to false
        isPaused = false;

        // Load scene
        SceneManager.LoadScene(toLoad);
    }

    // Upause the game
    public void Unpause()
    {
        // Reset the timeScale
        Time.timeScale = 1;

        // Set to false
        isPaused = false;

        // Set not in menu
        playerController.isInMenu = false;

        // Hide the menu
        pauseMenu.enabled = false;
    }
}
