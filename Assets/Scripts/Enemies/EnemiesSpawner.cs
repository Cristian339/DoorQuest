using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private int[] enemiesByLevel;
    [SerializeField] private GameObject[] enemiesSpawnPoints;

    private static int[] originalEnemiesByLevel;

    public int[] EnemiesByLevel { get => enemiesByLevel; set => enemiesByLevel = value; }

    public void Initialize()
    {
        originalEnemiesByLevel = (int[])enemiesByLevel.Clone();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enemies == null || enemiesByLevel == null || enemiesSpawnPoints == null)
        {
            Debug.LogError("Enemies, EnemiesByLevel, or EnemiesSpawnPoints is not assigned in the Inspector.");
            return;
        }

        for (int i = 0; i < enemies.Length; i++)
        {
            if (i >= enemiesSpawnPoints.Length)
            {
                Debug.LogWarning($"No spawn points configured for enemy index {i}");
                continue;
            }

            var spawnPoints = enemiesSpawnPoints[i].GetComponentsInChildren<Transform>().Where(x => x != enemiesSpawnPoints[i].transform).ToList();
            int maxEnemyNumber = spawnPoints.Count;
            Debug.Log($"Max enemy number for level {i}: {maxEnemyNumber}");

            for (int j = 0; j < Mathf.Min(enemiesByLevel[i], maxEnemyNumber); j++)
            {
                if (j >= spawnPoints.Count)
                {
                    Debug.LogWarning($"Not enough spawn points for enemy index {i} at spawn point {j}");
                    continue;
                }

                var spawnPoint = spawnPoints[j];
                if (spawnPoint == null)
                {
                    Debug.LogWarning($"Spawn point {j} for enemy index {i} is null.");
                    continue;
                }

                Enemy enemy = Instantiate(enemies[i], spawnPoint.position, Quaternion.identity);
                if (enemy == null)
                {
                    Debug.LogError($"Failed to instantiate enemy {i} at spawn point {j}");
                    continue;
                }

                // Asignar waypoints al enemigo
                enemy.WayPoints = spawnPoint.GetComponentsInChildren<Transform>().Where(x => x != spawnPoint).ToArray();
                Debug.Log($"Spawned enemy {i} at {spawnPoint.position}");
            }
        }
    }

    public void RestartSpawner()
    {
        enemiesByLevel = originalEnemiesByLevel;
    }
}