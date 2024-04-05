using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUp : MonoBehaviour
{
    private int count;
    public TextMeshProUGUI countText;

    void Start()
    {
        count = 0;
        SetCountText();
    }

   public void OnTriggerEnter (Collider other)
   {
       if (other.gameObject.CompareTag("Pickup"))
       {
           other.gameObject.SetActive(false);
           count = count + 1;
           SetCountText();
       }
   }

   public void SetCountText()
   {
       countText.text =  "Coins: " + count.ToString();
   }
}
