using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlavorText : MonoBehaviour
{
    [SerializeField] private string[] flavorTextOptions;
    [SerializeField] private TextMeshProUGUI flavorTextObject;

    public void DisplayRandomFlavorText()
    {
        //Choose a random string of text for the flavor text array and update the text
        int randomText = Random.Range(0, flavorTextOptions.Length);
        flavorTextObject.text = flavorTextOptions[randomText];
    }
}
