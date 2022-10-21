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
        if (LevelManager.instance.IsGameActive())
        {
            UpdateScrollSpeed();
            UpdateObstacleSpeeds();
            UpdateBackgroundSpeeds();
            UpdateAnimSpeed();
        }

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
        GetComponentInChildren<GrassScroll>().UpdateScrollSpeed(currentScrollSpeed);
    }

    private void UpdateAnimSpeed()
    {
        float percent = (speedometer.GetSpeed() / speedometer.GetMaxSpeed());
        carAnim.SetFloat("Speed", maxAnimSpeed * percent);
    }
}
