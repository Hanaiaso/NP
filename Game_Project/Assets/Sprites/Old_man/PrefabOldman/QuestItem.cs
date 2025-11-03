using UnityEngine;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
    [Header("Quest Item Data")]
    public string itemID; // GÕ TÊN VÀO ĐÂY: "Hat", "Wallet", "Key"
    public Sprite itemIcon; // KÉO ICON (Sprite) CỦA VẬT PHẨM VÀO ĐÂY

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // --- NÂNG CẤP LOGIC ---
            // Chỉ chạy code nếu QuestManager tồn tại VÀ quest đang diễn ra
            if (QuestManager.instance != null &&
                QuestManager.instance.currentState == QuestManager.QuestState.InProgress)
            {
                // Báo cho QuestManager biết vật phẩm này đã được nhặt
                QuestManager.instance.CollectItem(itemID, itemIcon);

                // Tắt vật phẩm này đi (biến mất)
                gameObject.SetActive(false);
            }
            // Nếu không, người chơi chỉ đi xuyên qua vật phẩm mà không có gì xảy ra
        }
    }
}