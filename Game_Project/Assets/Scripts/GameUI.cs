using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManage gameManage;
    public void StartGame()
    {
        gameManage.StartGame();
    }
    public void ContinueGame()
    {
        gameManage.ResumeGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
