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
        offset += currentScrollSpeed * Time.deltaTime;
    }

    private void UpdateObstacleSpeeds()
    {
        foreach(var i in FindObjectsOfType<Obstacle>())
        {
            i.SetMoveSpeed(speedometer.GetSpeed());
        }
    }

    private void UpdateScrollSpeed()
    {
        currentScrollSpeed = speedometer.GetSpeed() / (5 / scrollSpeed);
    }
}
