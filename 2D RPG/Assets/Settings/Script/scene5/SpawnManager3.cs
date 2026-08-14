using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnManager3 : MonoBehaviour
{
    public List<GameObject> targetList;

    public float spawnRate = 1.0f;

    private bool isGameActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(SpawnTargetRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StpoSpawn()
    {
        isGameActive = false;
    }
    public void StartSpawn(int difficulty)
    {
        spawnRate/= difficulty;
        isGameActive = true;
        StartCoroutine(SpawnTargetRoutine());
    }
    IEnumerator SpawnTargetRoutine()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnTarget();
        }
    }
    void SpawnTarget()
    {
        int index = Random.Range(0, targetList.Count);
        Instantiate(targetList[index], new Vector3(Random.Range(-7f, 7f), -6f, 0), targetList[index].transform.rotation);
    }
}
