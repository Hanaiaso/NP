using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gamePauseMenu;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject loseMenu;

    [Header("Cutscenes")]
    [SerializeField] private GameObject cutsceneCanvas;       // Cutscene mở đầu
    [SerializeField] private GameObject winCutsceneCanvas;    // Cutscene khi thắng
    [SerializeField] private GameObject loseCutsceneCanvas;   // Cutscene khi thua

    [SerializeField] private AudioManager audioManager;
    void Start()
    {
        MainMenu();
       
    }

    public void MainMenu()
    {
        mainMenu.SetActive(true);
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
        gamePauseMenu.SetActive(false);
        hud.SetActive(true);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        if (cutsceneCanvas != null) cutsceneCanvas.SetActive(false);
        if (winCutsceneCanvas != null) winCutsceneCanvas.SetActive(false);
        if (loseCutsceneCanvas != null) loseCutsceneCanvas.SetActive(false);

        Time.timeScale = 1f;
    }

    // ⏸️ Tạm dừng game
    public void GamePauseMenu()
    {
        mainMenu.SetActive(false);
        gamePauseMenu.SetActive(true);
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        Time.timeScale = 0f;
    }

    // 🏆 Khi thắng game
    public void WinGameMenu()
    {
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        if (winCutsceneCanvas != null)
        {
            winCutsceneCanvas.SetActive(true);
            Time.timeScale = 0f;
            CutsceneController cutscene = winCutsceneCanvas.GetComponent<CutsceneController>();
            cutscene.onCutsceneEnd = ShowWinMenu;
            cutscene.BeginCutscene();
        }
        else
        {
            ShowWinMenu();
        }
    }

    // ❌ Khi thua game
    public void LoseGameMenu()
    {
        hud.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        if (loseCutsceneCanvas != null)
        {
            loseCutsceneCanvas.SetActive(true);
            Time.timeScale = 0f;
            CutsceneController cutscene = loseCutsceneCanvas.GetComponent<CutsceneController>();
            cutscene.onCutsceneEnd = ShowLoseMenu;
            cutscene.BeginCutscene();
        }
        else
        {
            ShowLoseMenu();
        }
    }

    // ✅ Hiện menu thắng sau cutscene
    private void ShowWinMenu()
    {
        if (winCutsceneCanvas != null) winCutsceneCanvas.SetActive(false);
        winMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    // ✅ Hiện menu thua sau cutscene
    private void ShowLoseMenu()
    {
        if (loseCutsceneCanvas != null) loseCutsceneCanvas.SetActive(false);
        loseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
