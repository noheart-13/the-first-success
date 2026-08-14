using System.Collections.Generic;
using UnityEngine;

public sealed class SpawnObjectPool : MonoBehaviour
{
    private readonly Dictionary<GameObject, Queue<GameObject>> availableObjects = new();
    private readonly Dictionary<GameObject, GameObject> prefabByInstance = new();
    private bool initialized;

    public void Initialize(GameObject[] prefabs, int initialSizePerPrefab)
    {
        if (initialized)
        {
            return;
        }

        initialized = true;
        int poolSize = Mathf.Max(1, initialSizePerPrefab);

        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null || availableObjects.ContainsKey(prefab))
            {
                continue;
            }

            Queue<GameObject> pool = new();
            availableObjects.Add(prefab, pool);

            for (int i = 0; i < poolSize; i++)
            {
                pool.Enqueue(CreateInstance(prefab));
            }
        }
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            return null;
        }

        if (!availableObjects.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            availableObjects.Add(prefab, pool);
        }

        GameObject instance = pool.Count > 0 ? pool.Dequeue() : CreateInstance(prefab);
        instance.transform.SetPositionAndRotation(position, rotation);

        if (instance.TryGetComponent(out Rigidbody2D body))
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
        }

        instance.SetActive(true);

        if (instance.TryGetComponent(out AnimalController controller))
        {
            controller.BeginLifetime(this);
        }

        return instance;
    }

    public void Return(GameObject instance)
    {
        if (instance == null || !instance.activeSelf)
        {
            return;
        }

        if (!prefabByInstance.TryGetValue(instance, out GameObject prefab))
        {
            Destroy(instance);
            return;
        }

        if (instance.TryGetComponent(out Rigidbody2D body))
        {
            body.linearVelocity = Vector2.zero;
            body.angularVelocity = 0f;
        }

        instance.SetActive(false);
        instance.transform.SetParent(transform);
        availableObjects[prefab].Enqueue(instance);
    }

    private GameObject CreateInstance(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, transform);
        instance.name = prefab.name;
        prefabByInstance[instance] = prefab;
        instance.SetActive(false);
        return instance;
    }
}
