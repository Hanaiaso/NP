using UnityEngine;
using UnityEngine.UI; // Phải thêm cái này

public class QuestItem : MonoBehaviour
{
    [Header("Quest Item Data")]
    public string itemID; // GÕ TÊN VÀO ĐÂY: "Hat", "Wallet", "Key"
    public Sprite itemIcon; // KÉO ICON (Sprite) CỦA VẬT PHẨM VÀO ĐÂY

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (QuestManager.instance != null)
            {
                // Báo cho QuestManager biết vật phẩm này đã được nhặt
                // VÀ "đưa" icon của nó qua
                QuestManager.instance.CollectItem(itemID, itemIcon);
            }

            // Tắt vật phẩm này đi (biến mất)
            gameObject.SetActive(false);
        }
    }
}