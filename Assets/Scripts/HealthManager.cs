using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    public float currentHealth;
    public float smoothTime = 0.2f;
    public Gradient healthGradient;
    public TextMeshProUGUI healthText;

    public AudioSource death;

    private float targetFillAmount;
    private float velocity;

    void Start()
    {
        currentHealth = maxHealth;
        targetFillAmount = currentHealth / maxHealth;
        healthBar.fillAmount = targetFillAmount;
        healthBar.color = healthGradient.Evaluate(targetFillAmount);

        transform.position = GameManager.Instance.GetCurrentCheckpointPosition();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(10);
        }*/

        if (currentHealth <= 0)
        {
            Die();
        }

        healthBar.fillAmount = Mathf.SmoothDamp(healthBar.fillAmount, targetFillAmount, ref velocity, smoothTime);
        healthBar.color = healthGradient.Evaluate(healthBar.fillAmount);
        healthText.color = healthGradient.Evaluate(healthBar.fillAmount);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFillAmount = currentHealth / maxHealth;

        healthText.text = currentHealth.ToString("F0");
    }

    public void Heal(float healingAmount)
    {
        currentHealth += healingAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFillAmount = currentHealth / maxHealth;

        healthText.text = currentHealth.ToString("F0");
    }

    void Die() // Kill the player and respawn at last checkpoint
    {
        transform.position = GameManager.Instance.GetCurrentCheckpointPosition();
        Heal(maxHealth);
        death.Play();
    }
}
