using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenuUI;
    [SerializeField] GameObject gameOverLayer = null;

    public static bool gamePaused = false;


    //Sound
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip ImpLoughClips;
    [SerializeField] AudioClip GoblineLoughClips;

    public static Action OnGamePaused = () => { };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverLayer.activeSelf) return;
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

	private void OnEnable()
	{
        PlayerAttribute.OnPlayerDied += GameOverMenu;
	}

	private void OnDisable()
	{
        PlayerAttribute.OnPlayerDied -= GameOverMenu;
	}

	private void GameOverMenu()
	{
        gameOverLayer.SetActive(true);
        audioSource.PlayOneShot(GoblineLoughClips);
        audioSource.PlayOneShot(ImpLoughClips);
    }

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        OnGamePaused.Invoke();
    }
}
