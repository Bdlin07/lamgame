using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 5;

    private float timer;
    private int enemiesSpawned;

    private void Update()
    {
        if (enemyPrefab == null) return;
        if (enemiesSpawned >= maxEnemies) return;
        if (spawnInterval <= 0f) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemiesSpawned++;
        }
    }
}



