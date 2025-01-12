using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogFollower : MonoBehaviour
{
    [SerializeField] private Transform mainCamera; 
    [SerializeField] private float fixedYPosition = 8f; 
    [SerializeField] private float distance = 30f; 

    

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            Vector3 targetPosition = mainCamera.position + mainCamera.forward * distance;

            targetPosition.y = fixedYPosition;

            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0);
        }
    }
}