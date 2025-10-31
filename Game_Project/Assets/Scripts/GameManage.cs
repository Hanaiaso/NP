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
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

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
        Time.timeScale = 0f; // Pause the game
    }
    public void GameOverMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gamePauseMenu.SetActive(false);
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void GamePauseMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(true);
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(true);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    public void ResumeGame()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(true);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void WinGameMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(false);
        winMenu.SetActive(true);
        loseMenu.SetActive(false);
        Time.timeScale = 0f; // Pause the game
    }
    public void LoseGameMenu()
    {
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gamePauseMenu.SetActive(false);
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}
