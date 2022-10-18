using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float zRange;
    [SerializeField] private float speed;
    private Vector2 movement;
    private Rigidbody rb;

    [SerializeField] private float defaultAcceleration = 1;
    private float currentAcceleration;
    private IEnumerator currentObstacleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = defaultAcceleration;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(0, 0, -movement.x).normalized;

        rb.AddForce(direction * speed);

        if (transform.position.z < -zRange)
        {
            rb.velocity = Vector3.zero;
        }

        if (transform.position.z > zRange)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Obstacle currentObstacle = other.GetComponent<Obstacle>();
            
            if(currentObstacleCoroutine != null)
                StopCoroutine(currentObstacleCoroutine);

            //Act accordingly based on the type of obstacle
            switch (currentObstacle.obstacleType)
            {
                case ObstacleType.Decelerator:
                    currentObstacleCoroutine = ChangeCarAcceleration(currentObstacle);
                    StartCoroutine(currentObstacleCoroutine);
                    break;
                case ObstacleType.Accelerator:
                    currentObstacleCoroutine = ChangeCarAcceleration(currentObstacle);
                    StartCoroutine(currentObstacleCoroutine);
                    break;
                case ObstacleType.Visual:
                    FindObjectOfType<VisualEffectsManager>().ShowEffect(currentObstacle.GetVisualEvent());
                    break;
            }

            //Destroy the obstacle if the obstacle is marked to do so
            if(currentObstacle.DestroyOnCollide())
                Destroy(currentObstacle.gameObject);
        }
    }

    IEnumerator ChangeCarAcceleration(Obstacle currentObstacle)
    {
        float timeElapsed = 0;
        float startAcceleration = currentAcceleration;
        float endAcceleration = startAcceleration + currentObstacle.GetAccelerationChange();

        while (timeElapsed < currentObstacle.GetChangeSeconds())
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / currentObstacle.GetChangeSeconds();
            t = t * t * (3f - 2f * t);

            currentAcceleration = Mathf.Lerp(startAcceleration, endAcceleration, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        currentAcceleration = endAcceleration;

        //Slowly bring the player back to default acceleration
        currentObstacleCoroutine = ReturnToDefaultAcceleration(5);
        StartCoroutine(currentObstacleCoroutine);
    }

    IEnumerator ReturnToDefaultAcceleration(float seconds)
    {
        float timeElapsed = 0;
        float startAcceleration = currentAcceleration;
        float endAcceleration = defaultAcceleration;

        while (timeElapsed < seconds)
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / seconds;
            t = t * t * (3f - 2f * t);

            currentAcceleration = Mathf.Lerp(startAcceleration, endAcceleration, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        currentAcceleration = endAcceleration;
    }

    //Send value from Move callback to the horizontal Vector2
    public void OnMove(InputAction.CallbackContext ctx) => movement = ctx.ReadValue<Vector2>();
    public float GetAcceleration() => currentAcceleration;
}
