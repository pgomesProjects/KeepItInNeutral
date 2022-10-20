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
        isGameActive = true;
    }

    public void GameOver(PlayerController player)
    {
        isGameActive = false;

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

            switch (player.playerIndex)
            {
                case 1:
                    explosionPos.localPosition = new Vector3(-480, explosionPos.localPosition.y, explosionPos.localPosition.z);
                    break;
                case 2:
                    explosionPos.localPosition = new Vector3(480, explosionPos.localPosition.y, explosionPos.localPosition.z);
                    break;
            }

            FindObjectOfType<AudioManager>().PlayOneShot("CarExplosion", 0.5f);
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

        FindObjectOfType<AudioManager>().Stop("InGameOST");
        SceneManager.LoadScene("Titlescreen");
        Time.timeScale = 1.0f;
    }

    public bool IsGameActive() => isGameActive;
}
