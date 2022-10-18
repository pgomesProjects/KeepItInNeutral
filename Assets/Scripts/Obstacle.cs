using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ObstacleType { Decelerator, Accelerator, Visual }

public class Obstacle : MonoBehaviour
{
    public ObstacleType obstacleType;
    public UnityEvent visualEvent;

    [SerializeField] private float accelerationChange;
    [SerializeField] private float changeSeconds;
    [SerializeField] private bool destroyOnCollision = false;
    private float obstacleSpeed;

    private float destroyX = -20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(obstacleSpeed, 0, 0) * Time.deltaTime;

        if(transform.position.x <= destroyX)
            Destroy(gameObject);
    }

    public void SetMoveSpeed(float speed)
    {
        obstacleSpeed = speed;
    }

    public bool DestroyOnCollide() => destroyOnCollision;
    public float GetAccelerationChange() => accelerationChange;
    public float GetChangeSeconds() => changeSeconds;
}
