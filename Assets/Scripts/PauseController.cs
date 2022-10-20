using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    PlayerControls playerControls;

    private bool isPaused;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject menuArrow;
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
        StartCoroutine(PlayMusicAfterInit());
    }

    private IEnumerator PlayMusicAfterInit()
    {
        yield return new WaitForFixedUpdate();
        FindObjectOfType<AudioManager>().Play("InGameOST", 1);
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

    public void DisplaySelectArrow(float y)
    {
        RectTransform arrowTransform = menuArrow.GetComponent<RectTransform>();
        arrowTransform.anchoredPosition = new Vector2(arrowTransform.anchoredPosition.x, y);
    }

    public void TogglePause()
    {
        //If the game is active
        if (LevelManager.instance.IsGameActive())
        {
            //Toggle pause menu
            isPaused = !isPaused;

            //If the game is paused
            if (isPaused)
            {
                Time.timeScale = 0.0f;
                pauseMenu.SetActive(true);
                firstSelectedButton.Select();
            }
            //If the game is resumed
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
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
