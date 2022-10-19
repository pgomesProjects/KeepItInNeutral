using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject explosionObject;
    private bool isGameActive;
    private AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        isGameActive = true;
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void GameOver(PlayerController player)
    {
        isGameActive = false;

        StartCoroutine(DeathAnimation(player));
    }

    private IEnumerator DeathAnimation(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(3);

        //Destroy the player and play explosion
        if (audioManager != null)
        {
            explosionObject.SetActive(true);
            audioManager.PlayOneShot("CarExplosion", 0.5f);
            Destroy(player.gameObject);
        }

        yield return new WaitForSecondsRealtime(2);

        StartCoroutine(WaitForMainMenu(3));
    }

    private IEnumerator WaitForMainMenu(float seconds)
    {
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(seconds);

        SceneManager.LoadScene("Titlescreen");
        Time.timeScale = 1.0f;
    }

    public bool IsGameActive() => isGameActive;
}
