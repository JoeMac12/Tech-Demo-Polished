using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerTrigger : MonoBehaviour
{
    public GameObject textObject;
    public TextMeshProUGUI timerText;
    public float timerDuration = 150f;
    private bool timerStarted = false;

    public HealthManager playerHealth;
    public int damage = 9999;

    void Start()
    {
        textObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !timerStarted)
        {
            textObject.SetActive(true);

            timerStarted = true;
            StartCoroutine(CountdownTimer());
        }
    }

    IEnumerator CountdownTimer()
    {
        float timeRemaining = timerDuration;

        while (timeRemaining > 0 && timerStarted)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        if (timeRemaining <= 0) // uh idk restart level or kill player not sure
        {
            textObject.SetActive(false);
            Debug.Log("Time is out");
            timerStarted = false;
            playerHealth.TakeDamage(damage);
        }
    }
}
