using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // Thêm dòng này để điều khiển Image UI

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    // THÊM 1 TRẠNG THÁI MỚI: "ReadyToComplete"
    public enum QuestState { NotStarted, InProgress, ReadyToComplete, Completed }
    public QuestState currentState = QuestState.NotStarted;

    [Header("Quest Data")]
    public List<GameObject> questItemsInWorld; // Kéo 3 vật phẩm (từ Scene) vào đây

    [Header("Indicator Smoothing")]
    public float smoothSpeed = 5f;



    [Header("Target References")]
    public Transform player;
    public Transform oldManTransform; // KÉO ÔNG LÃO VÀO ĐÂY
    public Camera mainCamera;

    [Header("Quest UI")]
    public GameObject questUIPanel; // KÉO QuestUIPanel VÀO ĐÂY
    public Image[] questSlotsUI;    // KÉO 3 ô Slot1, Slot2, Slot3 VÀO ĐÂY
    //public Sprite itemCollectedIcon; // Sprite/Icon để hiển thị khi nhặt được
    public Color itemNotCollectedColor = new Color(1, 1, 1, 0.2f); // Màu cho ô trống
    public Color itemCollectedColor = Color.white;                 // Màu cho ô đã đầy
    public RectTransform questIndicator; // Mũi tên chỉ hướng

    private int itemsCollected = 0;
    private int totalItems = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Ban đầu, tắt mũi tên và giấu tất cả vật phẩm đi
        if (questIndicator != null)
            questIndicator.gameObject.SetActive(false);
        if (questUIPanel != null)
            questUIPanel.SetActive(false);

        // Cài đặt mặc định cho các ô slot
        foreach (Image slot in questSlotsUI)
        {
            slot.sprite = null; // Xóa icon
            slot.color = itemNotCollectedColor; // Đặt màu mờ
        }

        // --- NÂNG CẤP LOGIC ---
        // Giấu tất cả vật phẩm
        // Thay vì dùng List, chúng ta sẽ gán chúng bằng ID
        foreach (GameObject item in questItemsInWorld)
        {
            item.SetActive(false);
        }

        // Đếm tổng số vật phẩm
        totalItems = questItemsInWorld.Count;
        itemsCollected = 0; // Đảm bảo bắt đầu từ 0
    }

    void LateUpdate()
    {
        // Cập nhật mũi tên chỉ hướng
        if ((currentState == QuestState.InProgress || currentState == QuestState.ReadyToComplete)
            && questIndicator != null)
        {
            UpdateIndicator();
        }
    }

    public void StartQuest()
    {
        if (currentState != QuestState.NotStarted) return;

        currentState = QuestState.InProgress;
        itemsCollected = 0;

        // Kích hoạt tất cả vật phẩm
        foreach (GameObject item in questItemsInWorld)
        {
            item.SetActive(true);
        }

        // Bật UI (mũi tên và các ô slot)
        if (questIndicator != null)
            questIndicator.gameObject.SetActive(true);
        if (questUIPanel != null)
            questUIPanel.SetActive(true);

        Debug.Log("QUEST BẮT ĐẦU: Tìm " + totalItems + " vật phẩm!");
    }

    // Hàm này giờ nhận ID và Icon từ vật phẩm
    public void CollectItem(string itemID, Sprite icon)
    {
        if (currentState != QuestState.InProgress) return;

        // CẬP NHẬT UI SLOT
        // Logic mới: Tìm đúng ô (slot) để đặt icon vào
        if (itemID == "Hat")
        {
            questSlotsUI[0].sprite = icon; // Đặt icon vào ô 1
            questSlotsUI[0].color = itemCollectedColor;
        }
        else if (itemID == "Wallet")
        {
            questSlotsUI[1].sprite = icon; // Đặt icon vào ô 2
            questSlotsUI[1].color = itemCollectedColor;
        }
        else if (itemID == "Key")
        {
            questSlotsUI[2].sprite = icon; // Đặt icon vào ô 3
            questSlotsUI[2].color = itemCollectedColor;
        }

        itemsCollected++; // Vẫn đếm số lượng
        Debug.Log("Đã nhặt: " + itemID + "! (" + itemsCollected + "/" + totalItems + ")");

        // Logic MỚI: Nếu nhặt đủ, chuyển sang "Sẵn sàng trả quest"
        if (itemsCollected >= totalItems)
        {
            currentState = QuestState.ReadyToComplete;
            Debug.Log("Đã nhặt đủ! Hãy quay lại gặp ông lão.");
        }
    }

    // Hàm này sẽ được GỌI BỞI ÔNG LÃO
    public void CompleteQuest()
    {
        currentState = QuestState.Completed;

        // Tắt mũi tên và Quest UI
        if (questIndicator != null)
            questIndicator.gameObject.SetActive(false);
        if (questUIPanel != null)
            questUIPanel.SetActive(false);

        Debug.Log("QUEST HOÀN THÀNH!");
        // Bạn có thể thêm logic thưởng cho Player ở đây
    }

    // --- Hàm cho Mũi Tên Chỉ Hướng ---
    void UpdateIndicator()
    {
        Vector3 targetPosition;

        // ... (phần code tìm targetPosition của bạn giữ nguyên) ...
        if (currentState == QuestState.InProgress)
        {
            GameObject closestItem = GetClosestItem();
            if (closestItem == null) return;
            targetPosition = closestItem.transform.position;
        }
        else // (currentState == QuestState.ReadyToComplete)
        {
            if (oldManTransform == null) return;
            targetPosition = oldManTransform.position;
        }

        // --- BẮT ĐẦU NÂNG CẤP ---

        // 1. Tính toán hướng và góc xoay
        Vector3 targetDirection = targetPosition - player.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // 2. Tính toán vị trí
        Vector3 indicatorWorldPosition = player.position + new Vector3(0, 1.5f, 0);
        Vector3 targetScreenPosition = mainCamera.WorldToScreenPoint(indicatorWorldPosition);

        // 3. Áp dụng LERPM (Làm mượt)
        // Dùng Slerp để xoay mượt mà
        questIndicator.rotation = Quaternion.Slerp(questIndicator.rotation, targetRotation, Time.deltaTime * smoothSpeed);

        // Dùng Lerp để di chuyển mượt mà
        questIndicator.position = Vector3.Lerp(questIndicator.position, targetScreenPosition, Time.deltaTime * smoothSpeed);
    }

    GameObject GetClosestItem()
    {
        // ... (Hàm này giữ nguyên như cũ, không cần sửa) ...
        GameObject bestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = player.position;

        foreach (GameObject item in questItemsInWorld)
        {
            if (item == null || !item.activeInHierarchy) continue; // Bổ sung kiểm tra
            float distance = Vector3.Distance(item.transform.position, currentPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = item;
            }
        }
        return bestTarget;
    }
}