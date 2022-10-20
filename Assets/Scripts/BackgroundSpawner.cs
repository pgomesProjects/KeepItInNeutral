using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] backgroundPiecePrefabs;
    public GameObject backgroundParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnBackgroundPiece();
    }

    private void SpawnBackgroundPiece()
    {
        int randomBackgroundPiece = Random.Range(0, backgroundPiecePrefabs.Length);

        GameObject backgroundPiece = Instantiate(backgroundPiecePrefabs[randomBackgroundPiece], transform.position, transform.rotation);

        backgroundPiece.transform.parent = backgroundParent.transform;
    }
}
