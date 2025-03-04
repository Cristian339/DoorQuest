using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsSpawner : MonoBehaviour
{
    [SerializeField] private HealthPowerUp powerUpPrefab;
    [SerializeField] private Transform[] spawnPoints;

    void Start()
    {
        if (Random.Range(0, 10) > 6)
        {
            var transformToSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(powerUpPrefab, transformToSpawn.position, Quaternion.identity);
        }
    }
}
