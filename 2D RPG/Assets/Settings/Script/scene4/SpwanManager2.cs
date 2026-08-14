using UnityEngine;

public class SpwanManager2 : MonoBehaviour
{
    public GameObject enemyPrefabs;
    public GameObject powerupPerfabs;
    public float Range = 2f;
    public int numberOfEnemies = 1;

    void Start()
    {
        if (enemyPrefabs == null)
        {
            Debug.LogError("Enemy prefab is not assigned.", this);
            enabled = false;
            return;
        }

        if (powerupPerfabs == null)
        {
            Debug.LogError(
                "Powerup prefab is not assigned. Drag Assets/Prefabs/Power.prefab into the field.",
                this);
        }

        SpawnPowerupIfConfigured();
        SpawnRandomEnemy(numberOfEnemies);
    }

    private void Update()
    {
        int enemyCount = FindObjectsByType<EnemyControl>().Length;
        if (enemyCount == 0)
        {
            numberOfEnemies++;
            SpawnPowerupIfConfigured();
            SpawnRandomEnemy(numberOfEnemies);
        }
    }

    private void SpawnRandomEnemy(int number)
    {
        for (int i = 0; i < number; i++)
        {
            float randomX = Random.Range(-Range, Range);
            float randomY = Random.Range(-Range, Range);
            Vector3 position = new(randomX, randomY, 0f);
            Instantiate(enemyPrefabs, position, enemyPrefabs.transform.rotation);
        }
    }

    private void SpawnPowerupIfConfigured()
    {
        if (powerupPerfabs == null)
        {
            return;
        }

        float randomX = Random.Range(-Range, Range);
        float randomY = Random.Range(-Range, Range);
        Vector3 position = new(randomX, randomY, 0f);
        Instantiate(powerupPerfabs, position, Quaternion.identity);
    }
}
