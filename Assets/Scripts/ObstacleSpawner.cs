using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private float rangeZ;

    [SerializeField] private float minSpawnInterval = 5;
    [SerializeField] private float maxSpawnInterval = 10;
    private float currentTimer;
    private float currentSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        currentSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);   
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;

        if(currentTimer >= currentSpawnTime)
        {
            SpawnObstacle();
            currentTimer = 0;
            currentSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnObstacle()
    {
        int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
        float randomZ = Random.Range(-rangeZ, rangeZ);
        Vector3 randomPos = new Vector3(transform.position.x, transform.position.y, randomZ);

        Instantiate(obstaclePrefabs[randomObstacle], randomPos, obstaclePrefabs[randomObstacle].transform.rotation);
    }
}
