using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float currentScrollSpeed;
    private Renderer textureRend;
    private Material roadMat;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private ObstacleSpawner obstacleSpawner;

    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        textureRend = GetComponent<Renderer>();
        roadMat = textureRend.material;
        UpdateScrollSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScrollSpeed();
        UpdateObstacleSpeeds();
        UpdateBackgroundSpeeds();
        offset += currentScrollSpeed * Time.deltaTime;
        roadMat.mainTextureOffset = new Vector2(0, -offset);
    }

    private void UpdateObstacleSpeeds()
    {
        //Update the speeds of the obstacles in the obstacle spawner parent
        foreach(var i in obstacleSpawner.GetComponentsInChildren<Obstacle>())
        {
            i.SetMoveSpeed(speedometer.GetSpeed());
        }
    }

    private void UpdateBackgroundSpeeds()
    {
        //Update the speeds of the background pieces in the background parent
        foreach (var i in backgroundObject.GetComponentsInChildren<BackgroundPiece>())
        {
            i.SetMoveSpeed(speedometer.GetSpeed());
        }
    }

    private void UpdateScrollSpeed()
    {
        currentScrollSpeed = speedometer.GetSpeed() / (5 / scrollSpeed);
    }
}
