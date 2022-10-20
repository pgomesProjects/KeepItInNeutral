using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPiecePrefabs;
    public GameObject backgroundParent;
    [SerializeField] private float spawnTimer;
    public float timeToSpawn;
    Vector3 offSet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offSet = new Vector3 (0, 5, 0);
        spawnTimer += 1 * Time.deltaTime;

        timeToSpawn = Random.Range(1, 3);

        if (spawnTimer >= timeToSpawn)
        {
            SpawnBackgroundPiece();
            spawnTimer = 0;
        }
    }

    private void SpawnBackgroundPiece()
    {
        int randomBackgroundPiece = Random.Range(0, backgroundPiecePrefabs.Length);

        GameObject backgroundPiece = Instantiate(backgroundPiecePrefabs[randomBackgroundPiece], transform.position + offSet, transform.rotation);

        backgroundPiece.transform.parent = backgroundParent.transform;
    }
}
