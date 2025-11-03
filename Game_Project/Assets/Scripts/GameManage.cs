using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gamePauseMenu;
    [SerializeField] private GameObject hud;

    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        MainMenu();
       
    }
    public void MainMenu()
    {
        mainMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        if (cutsceneCanvas != null) cutsceneCanvas.SetActive(false);
        if (winCutsceneCanvas != null) winCutsceneCanvas.SetActive(false);
        if (loseCutsceneCanvas != null) loseCutsceneCanvas.SetActive(false);

        Time.timeScale = 0f;
        audioManager.StopAudioGame();
    }

    // 🎬 Khi nhấn Start Game
    public void StartGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(false);

        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(true);
           // Time.timeScale = 1f; // Cho phép cutscene chạy
            CutsceneController cutscene = cutsceneCanvas.GetComponent<CutsceneController>();
            cutscene.onCutsceneEnd = ResumeGameplay;
            cutscene.BeginCutscene();
            audioManager.PlayDefaultAudio();
        }
        else
        {
            ResumeGameplay();
        }
    }

    // 🎮 Khi cutscene mở đầu kết thúc hoặc game resume
    public void ResumeGameplay()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gamePauseMenu.SetActive(false);
        hud.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void GamePauseMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(true);
        hud.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f; // Resume the game
    }
    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(true);
        Time.timeScale = 1f; // Resume the game
    }

}
