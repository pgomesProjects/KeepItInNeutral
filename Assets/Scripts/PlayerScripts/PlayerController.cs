using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerIndex = 0;
    [SerializeField] private float speed;
    private Vector2 movement;
    private Rigidbody rb;

    [SerializeField] private float defaultAcceleration = 1;
    private float currentAcceleration;
    private float currentBoost;
    private IEnumerator currentObstacleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentAcceleration = defaultAcceleration;

        //Play car driving SFX loop
        FindObjectOfType<AudioManager>().Play("DrivingLoop", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    private void FixedUpdate()
    {
        //While the game is active, add force to the player's z axis to make them move left and right
        //Force is used to move the car in order to achieve the lagging effect of the car
        if (LevelManager.instance.IsGameActive())
        {
            Vector3 direction = new Vector3(0, 0, -movement.x).normalized;

            rb.AddForce(direction * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player collides with an obstacle
        if (other.CompareTag("Obstacle"))
        {
            Obstacle currentObstacle = other.GetComponent<Obstacle>();
            
            if(currentObstacleCoroutine != null)
                StopCoroutine(currentObstacleCoroutine);

            //Act accordingly based on the type of obstacle
            switch (currentObstacle.obstacleType)
            {
                //The player collided with a decelerator obstacle
                case ObstacleType.Decelerator:
                    currentObstacleCoroutine = ChangeCarAcceleration(currentObstacle);
                    StartCoroutine(currentObstacleCoroutine);
                    break;
                //The player collided with an accelerator obstacle
                case ObstacleType.Accelerator:
                    currentObstacleCoroutine = ChangeCarAcceleration(currentObstacle);
                    StartCoroutine(currentObstacleCoroutine);
                    break;
                //The player collided with a visual trap
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
        float endAcceleration = startAcceleration + currentObstacle.GetAccelerationChange() + currentBoost;
        currentBoost += currentObstacle.GetAccelerationChange();

        //If accelerating, play accelerating SFX
        if(currentObstacle.GetAccelerationChange() > 0)
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Accelerate", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
        //If not, play decelerating SFX
        else
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Decelerate", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }

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
        currentObstacleCoroutine = ReturnToDefaultAcceleration(1);
        StartCoroutine(currentObstacleCoroutine);
    }

    IEnumerator ChangeCarAcceleration(float accelerationChange, float seconds)
    {
        float timeElapsed = 0;
        float startAcceleration = currentAcceleration;
        float endAcceleration = startAcceleration + accelerationChange + currentBoost;
        currentBoost += accelerationChange;

        //If accelerating, play accelerating SFX
        if (accelerationChange > 0)
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Accelerate", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
        //If not, play decelerating SFX
        else
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Decelerate", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }

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

        //Slowly bring the player back to default acceleration
        currentObstacleCoroutine = ReturnToDefaultAcceleration(1);
        StartCoroutine(currentObstacleCoroutine);
    }

    IEnumerator ReturnToDefaultAcceleration(float seconds)
    {
        currentBoost = 0;
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

    public void OnAccelerate(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PowerBoost powerBoost = GetComponentInChildren<PowerBoost>();

            //If the player's power boost is ready and they choose to accelerate
            if (powerBoost != null && powerBoost.IsPowerUpReady())
            {
                currentObstacleCoroutine = ChangeCarAcceleration(10, 2);
                StartCoroutine(currentObstacleCoroutine);
                powerBoost.ResetBar();
            }
        }
    }

    public void OnDecelerate(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PowerBoost powerBoost = GetComponentInChildren<PowerBoost>();

            //If the player's power boost is ready and they choose to decelerate
            if (powerBoost != null && powerBoost.IsPowerUpReady())
            {
                currentObstacleCoroutine = ChangeCarAcceleration(-10, 2);
                StartCoroutine(currentObstacleCoroutine);
                powerBoost.ResetBar();
            }
        }
    }

    public float GetAcceleration() => currentAcceleration;
}
