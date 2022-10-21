using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPiecePrefabs;
    public GameObject backgroundParent;

    private BackgroundPiece mostRecentSpawn;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBackgroundPiece();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForNewSpawn();
    }

    private void SpawnBackgroundPiece()
    {
        //Pick a random background piece from the array
        int randomBackgroundPiece = Random.Range(0, backgroundPiecePrefabs.Length);

        //Adjust the spawn position so that it inherits the y position stored in the prefab before spawning it
        Vector3 spawnPosition = new Vector3(transform.position.x, backgroundPiecePrefabs[randomBackgroundPiece].transform.position.y, transform.position.z);
        mostRecentSpawn = Instantiate(backgroundPiecePrefabs[randomBackgroundPiece], spawnPosition, backgroundPiecePrefabs[randomBackgroundPiece].transform.rotation).GetComponent<BackgroundPiece>();

        //Move the background piece to the background parent
        mostRecentSpawn.transform.parent = backgroundParent.transform;
    }

    private void CheckForNewSpawn()
    {
        //If the most recently spawned piece is a defined distance away from the spawner, spawn a new background piece
        if(transform.position.x - mostRecentSpawn.transform.position.x > mostRecentSpawn.DistanceUntilSpawn())
        {
            SpawnBackgroundPiece();
        }
    }
}
