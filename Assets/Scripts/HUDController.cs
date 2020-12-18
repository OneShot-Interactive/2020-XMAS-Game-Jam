using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    // Public variables
    [Header("Referencing GameObjects")]
    public Image[] spiritSprites;
    public Image[] healthSprites;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the sprites
        foreach(Image spirit in spiritSprites)
        {
            spirit.enabled = false;
        }
    }

    // Update UI
    public void UpdateSpiritsUI(int spiritsPickedUp)
    {
        // Based on the amount
        switch(spiritsPickedUp)
        {
            case 0:
                spiritSprites[0].enabled = false;
                spiritSprites[1].enabled = false;
                spiritSprites[2].enabled = false;
                spiritSprites[3].enabled = false;
                break;

            case 1:
                spiritSprites[0].enabled = true;
                spiritSprites[1].enabled = false;
                spiritSprites[2].enabled = false;
                spiritSprites[3].enabled = false;
                break;

            case 2:
                spiritSprites[0].enabled = true;
                spiritSprites[1].enabled = true;
                spiritSprites[2].enabled = false;
                spiritSprites[3].enabled = false;
                break;

            case 3:
                spiritSprites[0].enabled = true;
                spiritSprites[1].enabled = true;
                spiritSprites[2].enabled = true;
                spiritSprites[3].enabled = false;
                break;

            case 4:
                spiritSprites[0].enabled = true;
                spiritSprites[1].enabled = true;
                spiritSprites[2].enabled = true;
                spiritSprites[3].enabled = true;
                break;
        }
    }

    // Update Health UI
    public void UpdateHealthUI(float health)
    {
        switch(health)
        {
            case 100:
                healthSprites[0].enabled = true;
                healthSprites[1].enabled = true;
                healthSprites[2].enabled = true;
                healthSprites[3].enabled = true;
                break;

            case 75:
                healthSprites[0].enabled = true;
                healthSprites[1].enabled = true;
                healthSprites[2].enabled = true;
                healthSprites[3].enabled = false;
                break;
            case 50:
                healthSprites[0].enabled = true;
                healthSprites[1].enabled = true;
                healthSprites[2].enabled = false;
                healthSprites[3].enabled = false;
                break;
            case 25:
                healthSprites[0].enabled = true;
                healthSprites[1].enabled = false;
                healthSprites[2].enabled = false;
                healthSprites[3].enabled = false;
                break;
            case 0:
                healthSprites[0].enabled = false;
                healthSprites[1].enabled = false;
                healthSprites[2].enabled = false;
                healthSprites[3].enabled = false;
                break;
        }
    }
}
