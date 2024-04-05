using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // Do this so we can set any points we want
    public Transform pointB;
    public float speed = 2.0f;

    private Vector3 nextPosition;

    void Start() // Start at A
    {
        nextPosition = pointA.position;
    }

    void Update() // Move the platform from the 2 points
    {
        if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
        {
            nextPosition = nextPosition == pointA.position ? pointB.position : pointA.position;
        }
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) // This is so that the player can move with the platform and not slide off
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other) // Stop moving with it
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
