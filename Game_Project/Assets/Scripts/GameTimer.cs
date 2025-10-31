using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeLimit = 600f; // 10 phút = 600 giây
    public TMP_Text timerText;     // gán trong Inspector
    private bool isGameOver = false;
    [SerializeField] private GameManage gameManager;
    void Update()
    {
        if (isGameOver) return;

        // Giảm thời gian
        timeLimit -= Time.deltaTime;

        // Cập nhật hiển thị
        int minutes = Mathf.FloorToInt(timeLimit / 60);
        int seconds = Mathf.FloorToInt(timeLimit % 60); 
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Khi hết thời gian
        if (timeLimit <= 0f)
        {
            timeLimit = 0f;
            GameOver();
        }
    }

    public void GameOver()
    {
        isGameOver = true;

        // Tìm đối tượng GameManage trong scene
        GameManage gameManage = FindObjectOfType<GameManage>();
        if (gameManage != null)
        {
            gameManage.LoseGameMenu(); // Gọi menu thua
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameManage trong scene!");
        }
    }

    // Gọi hàm này khi người chơi thắng boss cuối
    public void PlayerWin()
    {
        isGameOver = true;
        // Chuyển sang cutscene hoặc scene chiến thắng
        SceneManager.LoadScene("CutsceneScene");
    }
}
