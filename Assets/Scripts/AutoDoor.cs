using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public GameObject door;
    public float openSpeed = 5f;
    public float openHeight = 5f;

    private bool isOpening = false;
    private bool isClosing = false;
    private Vector3 closedPosition;

    public AudioSource doorSoundAudioSource;
    public AudioSource doorSoundCloseAudioSource;

    void Start()
    {
        if (door != null) // Getpos of door
        {
            closedPosition = door.transform.position;
        }
    }

    void Update()
    {
        if (isOpening) // Open door by moving it up
        {
            Vector3 newPosition = door.transform.position;
            newPosition.y = Mathf.Lerp(newPosition.y, closedPosition.y + openHeight, Time.deltaTime * openSpeed);
            door.transform.position = newPosition;

            if (door.transform.position.y >= closedPosition.y + openHeight) // Stop moving when nearing the end of the move pos
            {
                isOpening = false;
            }
        }
        else if (isClosing) // Now close it
        {
            Vector3 newPosition = door.transform.position;
            newPosition.y = Mathf.Lerp(newPosition.y, closedPosition.y, Time.deltaTime * openSpeed);
            door.transform.position = newPosition;

            if (door.transform.position.y <= closedPosition.y) // Stop closing
            {
                isClosing = false;
                door.transform.position = closedPosition;
            }
        }
    }

    void OnTriggerEnter(Collider other) // Open the door
    {
        if (other.CompareTag("Player"))
        {
            isOpening = true;
            isClosing = false;

            doorSoundAudioSource.Play();
        }
    }

    void OnTriggerExit(Collider other) // Close the door
    {
        if (other.CompareTag("Player"))
        {
            isOpening = false;
            isClosing = true;

            doorSoundCloseAudioSource.Play();
        }
    }
}
