using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManage gameManager; // 👈 Thêm dòng này

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Usb"))
        {
            gameManager.WinGameMenu(); // ✅ Gọi qua biến instance
            Destroy(collision.gameObject);
        }
    }
}
