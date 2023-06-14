using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Transform cam;


    // Update is called once per frame
    void LateUpdate()
    {
        // transform.LookAt(cam); // OK

        // Calculate the target rotation by adding 180 degrees to the camera's rotation
        Quaternion targetRotation = cam.rotation * Quaternion.Euler(0, 180, 0);

        // Apply the target rotation to the health bar UI object
        transform.rotation = targetRotation;

    }
}
