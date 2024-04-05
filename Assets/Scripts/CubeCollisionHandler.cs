using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollisionHandler : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
