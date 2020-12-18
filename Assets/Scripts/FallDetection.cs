using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetection : MonoBehaviour
{
    // On trigger enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Load scene
            SceneManager.LoadScene(0);
        } else if(collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        
    }
}
