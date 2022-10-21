using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CinemachineVirtualCamera playerCam;

    [SerializeField] private ParticleSystem carSmoke;

    [SerializeField] private float minFOV = 50;
    [SerializeField] private float maxFOV = 70;
    private float cameraFOV;

    //Min and Max Z rotation values
    private const float MIN_SPEED_ANGLE = 210;
    private const float MAX_SPEED_ANGLE = -20;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;

    [SerializeField] private float startingSpeed = 20;
    [SerializeField] private float speedMax;
    
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("Needle");
        speedLabelTemplateTransform = transform.Find("SpeedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = startingSpeed;

        CreateSpeedLabels();
        UpdateCameraFOV();
    }

    // Update is called once per frame
    void Update()
    {
        //When the game is active
        if (LevelManager.instance.IsGameActive())
        {
            //Slowly increase the speed
            speed += playerController.GetAcceleration() * Time.deltaTime;
            //Make sure the speed never goes outside the min and max speed bounds
            speed = Mathf.Clamp(speed, 0, speedMax);

            //Check to see if smoke must be shown
            CheckForSmoke();

            //Change camera FOV
            UpdateCameraFOV();

            //Rotate needle
            needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());

            //Check for game over
            CheckForGameOver();
        }
    }

    private void CheckForSmoke()
    {
        //If the player reaches over 85% of the maximum speed or under 8% of the maximum speed, show smoke
        if(speed > speedMax * 0.85f)
        {
            //If the smoke is not already playing, play the smoke particles
            if (!carSmoke.isPlaying)
            {
                carSmoke.Play();
            }
        }
        else if (speed < speedMax * 0.08f)
        {
            //If the smoke is not already playing, play the smoke particles
            if (!carSmoke.isPlaying)
            {
                carSmoke.Play();
            }
        }
        //Stop the smoke if neither conditions are met
        else if (carSmoke.isPlaying)
        {
            carSmoke.Stop();
        }
    }

    private void CreateSpeedLabels()
    {
        //The amount of speed labels to create
        int labelAmount = 10;

        //Total amount of angles possible in the speedometer
        float totalAngleSize = MIN_SPEED_ANGLE - MAX_SPEED_ANGLE;

        //Create all of the labels
        for(int i = 0; i <= labelAmount; i++)
        {
            //Spawn new label
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);

            //Figure out the angle for the label
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = MIN_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;

            //Rotate label
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);

            //Set the text number and make sure it isn't rotated like the dash image
            speedLabelTransform.Find("SpeedText").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(labelSpeedNormalized * speedMax).ToString();
            speedLabelTransform.Find("SpeedText").eulerAngles = Vector3.zero;

            //Show the label
            speedLabelTransform.gameObject.SetActive(true);
        }

        //Set the needle as the last sibling so that it gets drawn over the e finespeed labels
        needleTransform.SetAsLastSibling();
    }

    private float GetSpeedRotation()
    {
        //Determine the angle of the needle based on the min / max angle and the current speed
        float totalAngleSize = MIN_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = speed / speedMax;

        return MIN_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }

    private void UpdateCameraFOV()
    {
        //If the speed is not 0, slowly zoom out the field of view as the player gets faster
        if (speed != 0)
        {
            playerCam.m_Lens.FieldOfView = minFOV + ((maxFOV - minFOV) / (speedMax / speed));
        }
        else
            playerCam.m_Lens.FieldOfView = minFOV;
    }

    private void CheckForGameOver()
    {
        //If the speed reaches 0 or maximum, game over
        if (speed <= 0 || speed >= speedMax)
        {
            //Move the players to the middle of the road
            foreach(var i in FindObjectsOfType<PlayerController>())
            {
                i.transform.localPosition = new Vector3(i.transform.localPosition.x, i.transform.localPosition.y, 0);
            }
            LevelManager.instance.GameOver(playerController);
        }
    }

    public float GetSpeed() => speed;
    public float GetMaxSpeed() => speedMax;
}
