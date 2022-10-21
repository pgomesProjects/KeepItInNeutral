using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBoost : MonoBehaviour
{
    private Slider powerSlider;

    [SerializeField] private GameObject instructionsObject;
    [SerializeField] private float fillSeconds = 15;
    private float powerVal;
    private float increaseRate;
    private bool powerUpReady;
    private float targetValue = 14;

    private void Awake()
    {
        powerSlider = GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        powerUpReady = false;
        increaseRate = fillSeconds / targetValue;
    }

    // Update is called once per frame
    void Update()
    {
        //When the game is active
        if (LevelManager.instance.IsGameActive())
        {
            //Slowly add to the power boost slider value and update the slider if it is not full
            if (powerVal < targetValue)
            {
                powerVal += (1 / increaseRate) * Time.deltaTime;
                powerSlider.value = Mathf.Floor(powerVal);
            }
            //If the slider is full, let the game know and show instructions for the player
            else
            {
                powerUpReady = true;
                instructionsObject.SetActive(true);
            }
        }
    }

    public void ResetBar()
    {
        //Reset the power boost bar stats
        powerUpReady = false;
        powerSlider.value = 0;
        powerVal = 0;
        instructionsObject.SetActive(false);
    }

    public bool IsPowerUpReady() => powerUpReady;
}
