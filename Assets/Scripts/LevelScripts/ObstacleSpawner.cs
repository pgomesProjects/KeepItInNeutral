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
        //Timer system: spawn an obstacle between the minimum and maximum spawn interval
        currentTimer += Time.deltaTime;

        if (currentTimer >= currentSpawnTime)
        {
            SpawnObstacle();

            //Reset timer and generate a new spawn time
            currentTimer = 0;
            currentSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    private void SpawnObstacle()
    {
        //Generate a random obstacle, place it randomly along the z axis of the road, and spawn it
        int randomObstacle = Random.Range(0, obstaclePrefabs.Length);
        float randomZ = Random.Range(-rangeZ, rangeZ);
        Vector3 randomPos = new Vector3(transform.position.x, obstaclePrefabs[randomObstacle].transform.position.y, randomZ);

        GameObject newObstacle = Instantiate(obstaclePrefabs[randomObstacle], transform.parent, false);
        //Move obstacle to parent and local position
        newObstacle.transform.localPosition = randomPos;
        newObstacle.transform.parent = transform;
    }
}
