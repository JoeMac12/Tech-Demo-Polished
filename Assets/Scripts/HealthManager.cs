using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f;
    public float currentHealth;
    public float smoothTime = 0.2f;
    public Gradient healthGradient;

    private float targetFillAmount;
    private float velocity;

    void Start()
    {
        currentHealth = maxHealth;
        targetFillAmount = currentHealth / maxHealth;
        healthBar.fillAmount = targetFillAmount;
        healthBar.color = healthGradient.Evaluate(targetFillAmount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(10);
        }

        healthBar.fillAmount = Mathf.SmoothDamp(healthBar.fillAmount, targetFillAmount, ref velocity, smoothTime);
        healthBar.color = healthGradient.Evaluate(healthBar.fillAmount);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFillAmount = currentHealth / maxHealth;
    }

    public void Heal(float healingAmount)
    {
        currentHealth += healingAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetFillAmount = currentHealth / maxHealth;
    }
}
