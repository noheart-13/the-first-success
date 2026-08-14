using UnityEngine;

public class SpwanMananger : MonoBehaviour
{
    public float Xrange = 15f;
    public float spawnInterval = 1f;
    public float startDelay = 2f;
    public GameObject[] spawnPoints;

    [SerializeField, Min(1)] private int poolSizePerPrefab = 4;

    private SpawnObjectPool objectPool;

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("Spawn manager has no prefabs configured.", this);
            enabled = false;
            return;
        }

        objectPool = GetComponent<SpawnObjectPool>();
        if (objectPool == null)
        {
            objectPool = gameObject.AddComponent<SpawnObjectPool>();
        }

        objectPool.Initialize(spawnPoints, poolSizePerPrefab);

        InvokeRepeating(
            nameof(SpawnRandomObject),
            Mathf.Max(0f, startDelay),
            Mathf.Max(0.05f, spawnInterval));
    }

    void SpawnRandomObject()
    {
        int index = Random.Range(0, spawnPoints.Length);
        GameObject prefab = spawnPoints[index];
        if (prefab == null)
        {
            Debug.LogWarning($"Spawn prefab at index {index} is missing.", this);
            return;
        }

        float x = Random.Range(-Xrange, Xrange);
        objectPool.Get(prefab, new Vector2(x, 2.5f), Quaternion.identity);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(SpawnRandomObject));
    }
}
