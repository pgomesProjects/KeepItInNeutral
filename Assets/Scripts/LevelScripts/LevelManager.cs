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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Set the game to active and play the in-game music
        isGameActive = true;
        FindObjectOfType<AudioManager>().Play("InGameOST", PlayerPrefs.GetFloat("BGMVolume", 0.5f));
    }

    public void GameOver(PlayerController player)
    {
        //Make sure the game is inactive
        isGameActive = false;

        //Pause all sounds
        FindObjectOfType<AudioManager>().PauseAllSounds();

        //Remove visual effects
       FindObjectOfType<VisualEffectsManager>().InstantRemoveEffects();

        //Remove obstacles
        foreach (var i in FindObjectsOfType<Obstacle>())
            Destroy(i.gameObject);

        StartCoroutine(DeathAnimation(player));
    }

    private IEnumerator DeathAnimation(PlayerController player)
    {
        yield return new WaitForSecondsRealtime(3);

        //Destroy the player and play explosion
        if (FindObjectOfType<AudioManager>() != null)
        {
            explosionObject.SetActive(true);
            RectTransform explosionPos = explosionObject.GetComponent<RectTransform>();

            //When playing in VS mode, determine which player's half of the screen the explosion will go to
            switch (player.playerIndex)
            {
                case 1:
                    explosionPos.localPosition = new Vector3(-480, explosionPos.localPosition.y, explosionPos.localPosition.z);
                    break;
                case 2:
                    explosionPos.localPosition = new Vector3(480, explosionPos.localPosition.y, explosionPos.localPosition.z);
                    break;
            }

            //Play explosion sound effect and destroy the player
            FindObjectOfType<AudioManager>().PlayOneShot("CarExplosion", PlayerPrefs.GetFloat("SFXVolume", 0.5f));
            Destroy(player.gameObject);
        }

        yield return new WaitForSecondsRealtime(2);

        StartCoroutine(WaitForMainMenu(3));
    }

    private IEnumerator WaitForMainMenu(float seconds)
    {
        //Pause the game and show game over screen
        Time.timeScale = 0.0f;
        gameOverScreen.SetActive(true);

        //Wait a few seconds before going back to the main menu
        yield return new WaitForSecondsRealtime(seconds);

        FindObjectOfType<AudioManager>().Stop("InGameOST");
        SceneManager.LoadScene("Titlescreen");
        Time.timeScale = 1.0f;
    }

    public bool IsGameActive() => isGameActive;
}
