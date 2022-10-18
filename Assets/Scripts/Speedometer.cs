using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    //Min and Max Z rotation values
    private const float MIN_SPEED_ANGLE = 210;
    private const float MAX_SPEED_ANGLE = -20;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;

    private float speedMax;
    private float speed;

    private void Awake()
    {
        needleTransform = transform.Find("Needle");
        speedLabelTemplateTransform = transform.Find("SpeedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

        speed = 0;
        speedMax = 200;

        CreateSpeedLabels();
    }

    // Update is called once per frame
    void Update()
    {
        //speed += 30f * Time.deltaTime;

        speed = Mathf.Clamp(speed, 0, speedMax);

        //Rotate needle
        needleTransform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
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

        //Set the needle as the last sibling so that it gets drawn over the speed labels
        needleTransform.SetAsLastSibling();
    }

    private float GetSpeedRotation()
    {
        //Determine the angle of the needle based on the min / max angle and the current speed
        float totalAngleSize = MIN_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = speed / speedMax;

        return MIN_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}
