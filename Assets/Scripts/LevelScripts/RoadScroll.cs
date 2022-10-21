using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private float currentScrollSpeed;
    [SerializeField] private float maxAnimSpeed = 2.5f;
    private Renderer textureRend;
    private Material roadMat;
    [SerializeField] private Speedometer speedometer;
    [SerializeField] private GameObject backgroundObject;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private Animator carAnim;

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
        //Update the scroll speed of the road, the obstacles, and the car animation when the game is active
        if (LevelManager.instance.IsGameActive())
        {
            UpdateScrollSpeed();
            UpdateObstacleSpeeds();
            UpdateAnimSpeed();
        }

        //Update the background speeds
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
        //Update the scroll speed for the road and the grass
        currentScrollSpeed = speedometer.GetSpeed() / (5 / scrollSpeed);
        GetComponentInChildren<GrassScroll>().UpdateScrollSpeed(currentScrollSpeed);
    }

    private void UpdateAnimSpeed()
    {
        //Change the speed parameter on the car animator to adjust the speed of the animation based on the car speed
        float percent = (speedometer.GetSpeed() / speedometer.GetMaxSpeed());
        carAnim.SetFloat("Speed", maxAnimSpeed * percent);
    }
}
