using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodelockPuzzle : MonoBehaviour
{
    // Public variables
    [Header("Puzzle Settings")]
    public List<int> digitCode;
    public bool codeGenerated;

    [Header("Referencing GameObjects")]
    public Canvas codelockCanvas;

    [Header("Reward Settings")]
    public GameObject rewardObject;
    public Transform rewardSpawnPoint;
    public Vector3 rewardSpawnOffset;

    [Header("Referening Input Fields")]
    public InputField digitOneInput;
    public InputField digitTwoInput;
    public InputField digitThreeInput;
    public InputField digitFourInput;

    // Private variables
    private int digitOne, digitTwo, digitThree, digitFour;
    private int digitsCorrect = 0;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Hide the UI
        codelockCanvas.enabled = false;

        // Fetch PlayerController
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // Generate 4 digits
        for (int i = 0; i < 4; i++)
        {
            digitCode.Add(GenerateDigit());
        }
    }

    // Generate digit
    int GenerateDigit()
    {
        int Digit = Random.Range(0, 9);
        return Digit;
    }

    // Try the code the user has put in
    public void TryCode()
    {
        // Fetch the digits
        digitOne = int.Parse(digitOneInput.text);
        digitTwo = int.Parse(digitTwoInput.text);
        digitThree = int.Parse(digitThreeInput.text);
        digitFour = int.Parse(digitFourInput.text);

        // Try the code the user has inputted
        if(digitOne == digitCode[0])
        {
            digitsCorrect += 1;
        }

        if(digitTwo == digitCode[1])
        {
            digitsCorrect += 1;
        }

        if(digitThree == digitCode[2])
        {
            digitsCorrect += 1;
        }

        if(digitFour == digitCode[3])
        {
            digitsCorrect += 1;
        }

        // Success or Failure
        if (digitsCorrect == 4)
        {
            CodeSuccess();
        } else
        {
            // Reset the puzzle
            digitsCorrect = 0;

            digitOneInput.text = "";
            digitTwoInput.text = "";
            digitThreeInput.text = "";
            digitFourInput.text = "";

            // Hide the canvas
            codelockCanvas.enabled = false;

            // Set not in menu for player
            playerController.isInMenu = false;
        }
    }

    // Method for if the player gets the code right
    void CodeSuccess()
    {
        // Reward player
        Instantiate(rewardObject, rewardSpawnPoint.position + rewardSpawnOffset, Quaternion.identity);

        // Hide the canvas
        codelockCanvas.enabled = false;

        // Set not in menu for player
        playerController.isInMenu = false;
    }

    // Display the codelock
    public void DisplayCodelock()
    {
        // Enable canvas
        codelockCanvas.enabled = true;

        // Set is in menu for player
        playerController.isInMenu = true;
    }

    // Get digit
    public int GetDigit(int indexToLookAt)
    {
        // Return the code
        return digitCode[indexToLookAt];
    }
}
