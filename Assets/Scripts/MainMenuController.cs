using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Public variables
    [Header("Referencing Canvases")]
    public Canvas optionsMenu;
    public Canvas closeMenu;

    [Header("Referencing Video Settings")]
    public Dropdown videoResolutionDropdown;
    public Toggle videoFullscreenToggle;

    [Header("Referencing Audio Settings")]
    public Slider audioMainSlider;
    public Slider audioMusicSlider;

    [Header("Referencing GameObject")]
    public AudioSource backgroundMusic;
    public DiscordController discordController;
    public Text versionText;

    // Start is called before the first frame update
    void Start()
    {
        // Initially close all non important screens
        optionsMenu.enabled = false;
        closeMenu.enabled = false;

        // Set the inital volume
        backgroundMusic.volume = PlayerPrefs.GetFloat("Audio_MusicVolume");

        // Set the version text
        versionText.text = "V" + Application.version;
    }

    // Open options menu
    public void OpenOptions()
    {
        // Close other menus
        closeMenu.enabled = false;

        // Fullscreen
        bool fullscreenBool;

        if(PlayerPrefs.GetInt("Video_Fullscreen") == 1)
        {
            fullscreenBool = true;
        } else
        {
            fullscreenBool = false;
        }

        videoFullscreenToggle.isOn = fullscreenBool;

        // Resolution

        int resolutionVal = 0;

        switch(PlayerPrefs.GetInt("Video_ResolutionX"))
        {
            case 2560:
                resolutionVal = 0;
                break;

            case 1920:
                resolutionVal = 1;
                break;

            case 1280:
                resolutionVal = 2;
                break;
        }

        videoResolutionDropdown.value = resolutionVal;

        // Audio levels

        audioMainSlider.value = PlayerPrefs.GetFloat("Audio_MainVolume");
        audioMusicSlider.value = PlayerPrefs.GetFloat("Audio_MusicVolume");

        // Open the new menu
        optionsMenu.enabled = true;
    }

    // Close options menu
    public void CloseOptions()
    {
        // Close the menu
        optionsMenu.enabled = false;

        // Saving full screen
        int fullscreenBool = videoFullscreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("Video_Fullscreen", fullscreenBool);

        // Saving resolution
        switch(videoResolutionDropdown.value)
        {
            // 2560 x 1440
            case 0:
                PlayerPrefs.SetInt("Video_ResolutionX", 2560);
                PlayerPrefs.SetInt("Video_ResolutionY", 1440);

                break;

            // 1920 x 1080
            case 1:
                PlayerPrefs.SetInt("Video_ResolutionX", 1920);
                PlayerPrefs.SetInt("Video_ResolutionY", 1080);
                break;

            // 1280 x 720
            case 2:
                PlayerPrefs.SetInt("Video_ResolutionX", 1280);
                PlayerPrefs.SetInt("Video_ResolutionY", 720);
                break;
        }

        // Apply fullscreen
        Screen.fullScreen = videoFullscreenToggle.isOn;

        Screen.SetResolution(PlayerPrefs.GetInt("Video_ResolutionX"), PlayerPrefs.GetInt("Video_ResolutionY"), videoFullscreenToggle.isOn);

        // Save and set audio levels
        PlayerPrefs.SetFloat("Audio_MainVolume", audioMainSlider.value);
        PlayerPrefs.SetFloat("Audio_MusicVolume", audioMusicSlider.value);

        backgroundMusic.volume = PlayerPrefs.GetFloat("Audio_MusicVolume");

    }

    // Open exit menu
    public void OpenExitMenu()
    {
        // Close other menus
        CloseOptions();

        // Open the new menu
        closeMenu.enabled = true;
    }

    // Close exit menu
    public void CloseExitMenu()
    {
        // Close the menu
        closeMenu.enabled = false;
    }

    // Load a scene
    public void LoadScene(int loadScene)
    {
        // Load scene
        SceneManager.LoadScene(loadScene);
    }

    // Quit the game
    public void ExitGame()
    {
        Application.Quit(0);
    }
}
