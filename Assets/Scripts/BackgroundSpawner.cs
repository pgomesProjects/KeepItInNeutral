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
        int randomBackgroundPiece = Random.Range(0, backgroundPiecePrefabs.Length);

        Vector3 spawnPosition = new Vector3(transform.position.x, backgroundPiecePrefabs[randomBackgroundPiece].transform.position.y, transform.position.z);
        mostRecentSpawn = Instantiate(backgroundPiecePrefabs[randomBackgroundPiece], spawnPosition, transform.rotation).GetComponent<BackgroundPiece>();

        mostRecentSpawn.transform.parent = backgroundParent.transform;
    }

    private void CheckForNewSpawn()
    {
        if(transform.position.x - mostRecentSpawn.transform.position.x > mostRecentSpawn.DistanceUntilSpawn())
        {
            SpawnBackgroundPiece();
        }
    }
}
