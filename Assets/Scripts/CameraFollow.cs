using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public variables
    [Header("Camera Settings")]
    public Vector3 Offset;
    public bool enabledMaxHeightAndWidth;

    [Header("Camera Height Settings")]
    public float minHeight;
    public float maxHeight;

    [Header("Camera Horizontal Settings")]
    public float minHorizontal;
    public float maxHorizontal;

    [Header("Referencing GameObjects")]
    public Transform toFollow;

    // Update is called once per frame
    void Update()
    {
        if(enabledMaxHeightAndWidth)
        {
            transform.position = new Vector3(Mathf.Clamp(toFollow.position.x, minHorizontal, maxHorizontal), Mathf.Clamp(toFollow.position.y + Offset.y, minHeight, maxHeight), Offset.z);

        } else
        {
            transform.position = new Vector3(toFollow.position.x, toFollow.position.y + Offset.y, Offset.z);
        }
    }
}
