using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public enum TITLESCREEN {STARTSCREEN, MAINMENU, OPTIONS};

    [SerializeField] private GameObject menuArrow;
    [SerializeField] private GameObject[] titleMenus;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.UI.Quit.performed += _ => QuitGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainMenu(Button mainButton)
    {
        //Hide the start menu and show the main menu
        titleMenus[(int)TITLESCREEN.STARTSCREEN].SetActive(false);
        titleMenus[(int)TITLESCREEN.MAINMENU].SetActive(true);

        //Highlight the first button
        mainButton.Select();
    }

    public void DisplaySelectArrow(float y)
    {
        RectTransform arrowTransform = menuArrow.GetComponent<RectTransform>();
        arrowTransform.anchoredPosition = new Vector2(arrowTransform.anchoredPosition.x, y);
    }

    public void PlayGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
