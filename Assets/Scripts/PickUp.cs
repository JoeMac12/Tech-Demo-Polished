using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUp : MonoBehaviour
{
    private int count;
    public TextMeshProUGUI countText;
    public AudioSource pickupSound;

    void Start()
    {
        count = 0;
        SetCountText();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            pickupSound.Play();
        }
    }

    public void SetCountText()
    {
        countText.text = "Coins: " + count.ToString() + " / 8";
    }
}
