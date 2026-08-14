using UnityEngine;

public class WaveMananger : MonoBehaviour
{
    public float spawnRate = 1.5f;
    public float spawnDelay = 0f;
    public GameObject Obstacle;

    private player playerScript;

    void Start()
    {
        playerScript = FindAnyObjectByType<player>();
        if (playerScript == null || Obstacle == null)
        {
            Debug.LogError("Wave manager requires both a player and an obstacle prefab.", this);
            enabled = false;
            return;
        }

        RestartSpawning();
    }

    void SpawnObstacle()
    {
        if (playerScript.isGameOver)
        {
            StopSpawning();
            return;
        }

        Instantiate(Obstacle, new Vector3(10f, -2.7f, 0f), Quaternion.identity);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnObstacle));
    }

    public void RestartSpawning()
    {
        StopSpawning();

        if (!enabled || playerScript == null || Obstacle == null)
        {
            return;
        }

        InvokeRepeating(
            nameof(SpawnObstacle),
            Mathf.Max(0f, spawnDelay),
            Mathf.Max(0.05f, spawnRate));
    }

    void OnDisable()
    {
        StopSpawning();
    }
}
