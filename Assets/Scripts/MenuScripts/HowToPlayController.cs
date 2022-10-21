using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private string[] tutorialLines;
    [SerializeField] private GameObject[] advanceObjects;

    private int currentLine;

    private PlayerControls playerControls;
    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.Previous.performed += _ => PreviousButton();
        playerControls.UI.Next.performed += _ => NextButton();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        SetupTutorial();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void SetupTutorial()
    {
        //Move to the first diagram / line
        currentLine = 0;
        UpdateUI();
    }

    public void PreviousButton()
    {
        //Move to the previous diagram / line
        if(currentLine > 0)
        {
            currentLine--;
            UpdateUI();
        }
    }

    public void NextButton()
    {
        //Move to the next diagram / line
        if (currentLine < tutorialLines.Length - 1)
        {
            currentLine++;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        //If the current line is in the length of the array, update the text
        if (currentLine < tutorialLines.Length)
            tutorialText.text = tutorialLines[currentLine];

        //If at the first line, hide the previous button
        if (currentLine <= 0)
            advanceObjects[0].SetActive(false);
        else
            advanceObjects[0].SetActive(true);

        //If at the last line, hide the next button
        if (currentLine >= tutorialLines.Length - 1)
            advanceObjects[1].SetActive(false);
        else
            advanceObjects[1].SetActive(true);
    }
}
