using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainbow : MonoBehaviour
{
    public float rotationSpeed = 50.0f;
    public float colorChangeSpeed = 1.0f;
    private float hue = 0.0f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // rotate the cube

        hue += colorChangeSpeed * Time.deltaTime;
        hue %= 1.0f;
        GetComponent<Renderer>().material.color = Color.HSVToRGB(hue, 1, 1); // modify the componet with hue color changing for awesome rainbow cube
    }
}
