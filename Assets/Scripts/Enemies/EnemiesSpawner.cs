using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
        for (int i = 0; i < enemies.Length; i++)
        {
            int maxEnemyNumber = enemiesSpawnPoints[i].GetComponentsInChildren<Transform>().Where(x => x.CompareTag("SpawnPoint")).Count();

            for (int j = 0; j < Mathf.Min(enemiesByLevel[i], maxEnemyNumber); j++)
            {
                var spawnPoint = enemiesSpawnPoints[i].transform.GetComponentsInChildren<Transform>().Where(x => x.CompareTag("SpawnPoint")).ToList()[j];
                Enemy enemy = Instantiate(enemies[i], spawnPoint.position, Quaternion.identity);
                enemy.WayPoints = spawnPoint.GetComponentsInChildren<Transform>().Where(x => x != spawnPoint).ToArray();
            }
        }
    }

    public void RestartSpawner()
    {
        enemiesByLevel = originalEnemiesByLevel;
    }
}
