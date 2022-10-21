using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public enum PAUSESCREEN { PAUSE, OPTIONS };

    PlayerControls playerControls;

    private bool isPaused;
    [SerializeField] private GameObject mainPauseMenu;
    [SerializeField] private GameObject[] pauseMenus;
    private GameObject menuArrow;
    [SerializeField] private Button firstSelectedButton;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Pause.performed += _ => TogglePause();
    }

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
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

    public void TogglePause()
    {
        //If the game is active
        if (LevelManager.instance.IsGameActive())
        {
            //If the pause menu is currently on the options menu, bring it back to the options menu
            if (pauseMenus[(int)PAUSESCREEN.OPTIONS].activeInHierarchy)
            {
                CancelOptions();
            }
            else
            {
                //Toggle pause menu
                isPaused = !isPaused;

                //If the game is paused
                if (isPaused)
                {
                    Time.timeScale = 0.0f;
                    mainPauseMenu.SetActive(true);
                    firstSelectedButton.Select();
                    FindObjectOfType<AudioManager>().PauseAllSounds();
                }
                //If the game is resumed
                else
                {
                    mainPauseMenu.SetActive(false);
                    Time.timeScale = 1.0f;
                    FindObjectOfType<AudioManager>().ResumeAllSounds();
                }
            }
        }
    }

    public void OpenOptionsMenu(Slider optionSlider)
    {
        //Hide the main menu and show the options menu
        pauseMenus[(int)PAUSESCREEN.PAUSE].SetActive(false);
        pauseMenus[(int)PAUSESCREEN.OPTIONS].SetActive(true);

        //Refresh the options menu objects
        FindObjectOfType<SettingsController>().RefreshMenu();

        //Highlight the first slider
        optionSlider.Select();
    }

    public void CancelOptions()
    {
        //Hide the options menu and show the main menu
        pauseMenus[(int)PAUSESCREEN.PAUSE].SetActive(true);
        pauseMenus[(int)PAUSESCREEN.OPTIONS].SetActive(false);

        //Highlight the first button
        firstSelectedButton.Select();
    }

    public void ReturnToMain()
    {
        //Go back to the main menu
        FindObjectOfType<AudioManager>().Stop("InGameOST");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Titlescreen");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
