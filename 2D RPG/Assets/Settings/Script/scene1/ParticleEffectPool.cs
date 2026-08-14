using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ParticleEffectPool : MonoBehaviour
{
    private static readonly Dictionary<GameObject, ParticleEffectPool> Pools = new();

    private readonly Queue<GameObject> availableEffects = new();
    private GameObject template;

    public static ParticleEffectPool GetOrCreate(GameObject effectTemplate, int initialSize = 3)
    {
        if (effectTemplate == null)
        {
            return null;
        }

        if (Pools.TryGetValue(effectTemplate, out ParticleEffectPool existingPool) && existingPool != null)
        {
            return existingPool;
        }

        GameObject poolObject = new($"{effectTemplate.name} Pool");
        ParticleEffectPool pool = poolObject.AddComponent<ParticleEffectPool>();
        pool.Initialize(effectTemplate, Mathf.Max(1, initialSize));
        Pools[effectTemplate] = pool;
        return pool;
    }

    public void PlayAt(Vector3 position)
    {
        GameObject effect = availableEffects.Count > 0
            ? availableEffects.Dequeue()
            : CreateEffect();

        effect.transform.SetPositionAndRotation(position, Quaternion.identity);
        effect.SetActive(true);

        ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>(true);
        float playbackDuration = 0.1f;

        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particles.Play(true);

            ParticleSystem.MainModule main = particles.main;
            float systemDuration = main.startDelay.constantMax
                + main.duration
                + main.startLifetime.constantMax;
            playbackDuration = Mathf.Max(playbackDuration, systemDuration);
        }

        StartCoroutine(ReturnAfterDelay(effect, playbackDuration));
    }

    private void Initialize(GameObject effectTemplate, int initialSize)
    {
        template = effectTemplate;
        template.SetActive(false);

        for (int i = 0; i < initialSize; i++)
        {
            availableEffects.Enqueue(CreateEffect());
        }
    }

    private GameObject CreateEffect()
    {
        GameObject effect = Instantiate(template, transform);
        effect.name = template.name;
        effect.SetActive(false);
        return effect;
    }

    private IEnumerator ReturnAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (effect == null)
        {
            yield break;
        }

        ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem particles in particleSystems)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        effect.SetActive(false);
        effect.transform.SetParent(transform);
        availableEffects.Enqueue(effect);
    }

    private void OnDestroy()
    {
        if (!ReferenceEquals(template, null)
            && Pools.TryGetValue(template, out ParticleEffectPool pool)
            && pool == this)
        {
            Pools.Remove(template);
        }
    }
}
