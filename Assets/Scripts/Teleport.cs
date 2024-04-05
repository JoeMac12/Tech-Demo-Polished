using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered!");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!");
            other.transform.position = teleportDestination.position;
            Debug.Log("Player teleported to destination!");
        } else {
            Debug.Log("Object entered trigger, but it wasn't the Player.");
        }
    }
}
