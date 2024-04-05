using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] GameObject cubePrefab;
    [SerializeField] float spawnInterval = 2f;
    [SerializeField] float cubeSpeed = 5f;
    [SerializeField] float damage = 4f;
    [SerializeField] float cubeLifetime = 4f;


    float timeSinceLastSpawn;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnCube();
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnCube()
    {
        Vector3 spawnPosition = Random.insideUnitSphere * 5f + transform.position;

        GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 direction = (playerPosition - spawnPosition).normalized;

        cube.GetComponent<Rigidbody>().AddForce(direction * cubeSpeed, ForceMode.Impulse);

        Collider cubeCollider = cube.GetComponent<Collider>();

        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            Physics.IgnoreCollision(cubeCollider, boss.GetComponent<Collider>());
        }

        cube.AddComponent<CubeCollisionHandler>().damage = damage;

        Destroy(cube, cubeLifetime);
    }
}
