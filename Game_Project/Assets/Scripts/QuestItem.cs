using UnityEngine;

public class QuestItem : MonoBehaviour
{
    // Bạn có thể thêm ID hoặc tên vật phẩm ở đây nếu muốn
    // public string itemID = "wallet"; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải Player va chạm không
        if (other.CompareTag("Player"))
        {
            // Báo cho QuestManager biết vật phẩm này đã được nhặt
            if (QuestManager.instance != null)
            {
                QuestManager.instance.CollectItem(this.gameObject);
            }

            // Tắt vật phẩm này đi (biến mất)
            gameObject.SetActive(false);
        }
    }
}