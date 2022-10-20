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
    private float targetValue = 100;

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
        if (LevelManager.instance.IsGameActive())
        {
            if (powerVal < targetValue)
            {
                powerVal += (1 / increaseRate) * Time.deltaTime;
                powerSlider.value = powerVal;
            }
            else
            {
                powerUpReady = true;
                instructionsObject.SetActive(true);
            }
        }
    }

    public void ResetBar()
    {
        powerUpReady = false;
        powerSlider.value = 0;
        powerVal = 0;
        instructionsObject?.SetActive(false);
    }

    public bool IsPowerUpReady() => powerUpReady;
}
