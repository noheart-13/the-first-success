using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject vrx;
    [SerializeField, Min(1)] private int poolSize = 3;

    private ParticleEffectPool effectPool;

    private void Start()
    {
        effectPool = ParticleEffectPool.GetOrCreate(vrx, poolSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (effectPool == null)
        {
            effectPool = ParticleEffectPool.GetOrCreate(vrx, poolSize);
        }

        effectPool?.PlayAt(transform.position);
        Destroy(gameObject);
    }
}
