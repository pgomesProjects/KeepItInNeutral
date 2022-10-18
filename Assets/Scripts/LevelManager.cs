using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        instance = this;
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);

        StartCoroutine(WaitForMainMenu(3));
    }

    private IEnumerator WaitForMainMenu(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

        SceneManager.LoadScene("Titlescreen");
        Time.timeScale = 1.0f;
    }

}
