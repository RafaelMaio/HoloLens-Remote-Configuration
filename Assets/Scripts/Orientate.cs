// ===============================
// AUTHOR     : Rafael Maio (rafael.maio@ua.pt)
// PURPOSE     : Orientante the button to face the camera.
// SPECIAL NOTES: X
// ===============================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientate : MonoBehaviour
{
    /// <summary>
    /// Camera - To access its pose.
    /// </summary>
    private GameObject cam;

    /// <summary>
    /// Object (cube) initial rotation.
    /// </summary>
    private Vector3 initialRotation;

    /// <summary>
    /// Unity Start function.
    /// Stores the initial rotation.
    /// </summary>
    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        initialRotation = transform.rotation.eulerAngles;
    }

    /// <summary>
    /// Unity Update function.
    /// </summary>
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y + 90, 0);
    }

    /// <summary>
    /// Get the initial object rotation.
    /// </summary>
    /// <returns>The initial object rotation stored.</returns>
    public Vector3 getInitialRotation()
    {
        return initialRotation;
    }
}