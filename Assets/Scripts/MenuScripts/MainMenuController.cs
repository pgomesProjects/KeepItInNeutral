using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public enum TITLESCREEN {STARTSCREEN, MAINMENU, HOWTOPLAY, OPTIONS, CREDITS};

    private GameObject menuArrow;
    [SerializeField] private GameObject[] titleMenus;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("MainMenuOST", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void OpenMainMenu(Button mainButton)
    {
        //Hide any other menu and show the main menu
        titleMenus[(int)TITLESCREEN.STARTSCREEN].SetActive(false);
        titleMenus[(int)TITLESCREEN.HOWTOPLAY].SetActive(false);
        titleMenus[(int)TITLESCREEN.OPTIONS].SetActive(false);
        titleMenus[(int)TITLESCREEN.CREDITS].SetActive(false);
        titleMenus[(int)TITLESCREEN.MAINMENU].SetActive(true);

        //Change the flavor text
        FindObjectOfType<FlavorText>().DisplayRandomFlavorText();

        //Highlight the first button
        mainButton.Select();
    }

    public void OpenHowToPlayMenu(Button backButton)
    {
        //Hide the main menu and show the credits menu
        titleMenus[(int)TITLESCREEN.MAINMENU].SetActive(false);
        titleMenus[(int)TITLESCREEN.HOWTOPLAY].SetActive(true);

        //Highlight the back button
        backButton.Select();
    }

    public void OpenOptionsMenu(Slider optionSlider)
    {
        //Hide the main menu and show the options menu
        titleMenus[(int)TITLESCREEN.MAINMENU].SetActive(false);
        titleMenus[(int)TITLESCREEN.OPTIONS].SetActive(true);

        //Refresh the options menu objects
        FindObjectOfType<SettingsController>().RefreshMenu();

        //Highlight the first slider
        optionSlider.Select();
    }

    public void OpenCreditsMenu(Button backButton)
    {
        //Hide the main menu and show the credits menu
        titleMenus[(int)TITLESCREEN.MAINMENU].SetActive(false);
        titleMenus[(int)TITLESCREEN.CREDITS].SetActive(true);

        //Highlight the back button
        backButton.Select();
    }

    public void DisplaySelectArrow(float y)
    {
        //Move the select arrow to the y position of the selected menu button
        RectTransform arrowTransform = menuArrow.GetComponent<RectTransform>();
        arrowTransform.anchoredPosition = new Vector2(arrowTransform.anchoredPosition.x, y);
    }

    public void SetSelectArrow(GameObject arrow)
    {
        menuArrow = arrow;
    }

    public void PlayGame(string levelName)
    {
        //Stop the main menu music and load the game level scene
        FindObjectOfType<AudioManager>().Stop("MainMenuOST");
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        //If the game is playing in the Unity Editor, stop the Unity play session
        //Anything in these brackets will not be compiled in the game's build
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
