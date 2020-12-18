using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public CodelockPuzzle codelockController;
    [Range(0, 3)] public int codelockNumber;
    public int numberToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getNumber());
    }

    // Fetch the number form the codelockController class
    IEnumerator getNumber()
    {
        yield return new WaitWhile(() => codelockController.codeGenerated);

        numberToDisplay = codelockController.GetDigit(codelockNumber);
    }
}
