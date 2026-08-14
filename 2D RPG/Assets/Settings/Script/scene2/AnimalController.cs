using System.Collections;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField, Min(1f)] private float lifetime = 15f;

    private SpawnObjectPool ownerPool;
    private Coroutine lifetimeCoroutine;

    public void BeginLifetime(SpawnObjectPool pool)
    {
        ownerPool = pool;

        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }

        lifetimeCoroutine = StartCoroutine(ReturnAfterLifetime());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("shoot"))
        {
            return;
        }

        if (ownerPool != null)
        {
            ownerPool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Destroy(other.gameObject);
    }

    private IEnumerator ReturnAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);

        if (ownerPool != null)
        {
            ownerPool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }
    }
}
