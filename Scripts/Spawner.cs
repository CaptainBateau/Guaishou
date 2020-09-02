using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject thingToSpawn;
    [SerializeField] bool autoSpawn;
    [SerializeField] float delay = 6;
    [SerializeField] float randomRangeDelay = 8;
    bool isSpawning = false;
    float timer = 0;
    float actualDelay;

    public void Spawn()
    {
        Instantiate(thingToSpawn);
    }
    public void StartSpawning()
    {
        isSpawning = true;
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    private void Start()
    {
        actualDelay = delay;
        if (autoSpawn)
            isSpawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > actualDelay && isSpawning)
        {
            actualDelay = Random.Range(delay - randomRangeDelay / (float)2, delay + randomRangeDelay / (float)2);
            Spawn();
            timer = 0;
        }
    }
}
