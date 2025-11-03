using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OldManInteraction : MonoBehaviour
{
    [Header("Indicator")]
    public GameObject interactCanvas;
    public KeyCode interactKey = KeyCode.E;

    [Header("Dialogue Data")]
    // CHÚNG TA CẦN 4 ĐOẠN HỘI THOẠI
    public Conversation startQuestConversation;    // 1. Lời thoại GIAO QUEST (8 câu của bạn)
    public Conversation questInProgressConversation; // 2. Lời thoại KHI ĐANG LÀM (VD: "Cháu tìm thấy chưa?")
    public Conversation questCompletionConversation; // 3. Lời thoại TRẢ QUEST (VD: "Ôi cảm ơn cháu!")
    public Conversation postQuestConversation;       // 4. Lời thoại SAU KHI XONG (VD: "Cảm ơn cháu lần nữa")

    [Header("Quest Reward")]
    public GameObject chestPrefab;

    private Animator oldManAnimator;

    [Header("Optional")]
    public bool shouldLockPlayerWhileTalking = true;
    public string playerTag = "Player";

    private bool playerInRange = false;
    private GameObject playerObject;
    private bool isTalking = false;
    private bool isFinishingQuest = false;// THÊM BIẾN "CỜ" NÀY

    // ... (Hàm Start, Update, OnTriggerEnter2D, OnTriggerExit2D giữ nguyên) ...
    void Start()
    {
        if (interactCanvas != null)
            interactCanvas.SetActive(false);

        oldManAnimator = GetComponent<Animator>();
        if (oldManAnimator == null)
            oldManAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Khối này chạy KHI NGƯỜI CHƠI BẤM E
        if (playerInRange && Input.GetKeyDown(interactKey) && !isTalking)
        {
            StartDialogue();
        }

        // Khối này chạy KHI HỘI THOẠI VỪA KẾT THÚC
        if (isTalking &&
            DialogueManager.instance != null &&
            !DialogueManager.instance.dialoguePanel.activeInHierarchy)
        {
            isTalking = false; // Reset cờ hội thoại

            // Bật lại "Press E" nếu player vẫn đứng gần
            if (playerInRange && interactCanvas != null)
                interactCanvas.SetActive(true);


            // --- THÊM LOGIC MỚI VÀO ĐÂY ---
            // Kiểm tra xem chúng ta có vừa kết thúc quest không?
            if (isFinishingQuest)
            {
                // 1. Chính thức hoàn thành quest (để tắt UI, v.v.)
                QuestManager.instance.CompleteQuest();

                // 2. Tung phần thưởng
                SpawnReward();

                // 3. Ông lão biến mất
                gameObject.SetActive(false);
            }
            // --- KẾT THÚC LOGIC MỚI ---
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerObject = other.gameObject;
            if (interactCanvas != null && !isTalking)
                interactCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            playerObject = null;
            if (interactCanvas != null)
                interactCanvas.SetActive(false);

            if (isTalking && DialogueManager.instance != null)
            {
                DialogueManager.instance.EndDialogue();
                isTalking = false;
            }
        }
    }
    // ... (Hàm Start, Update, OnTriggerEnter2D, OnTriggerExit2D giữ nguyên) ...


    // HÀM QUAN TRỌNG NHẤT: STARTDIALOGUE
    private void StartDialogue()
    {
        isTalking = true;
        if (interactCanvas != null)
            interactCanvas.SetActive(false);

        // Logic MỚI: Chọn lời thoại dựa trên 4 trạng thái
        Conversation conversationToStart = null;
        QuestManager.QuestState currentState = QuestManager.instance.currentState;

        if (currentState == QuestManager.QuestState.NotStarted)
        {
            conversationToStart = startQuestConversation;
            QuestManager.instance.StartQuest();
        }
        else if (currentState == QuestManager.QuestState.InProgress)
        {
            conversationToStart = questInProgressConversation;
        }
        else if (currentState == QuestManager.QuestState.ReadyToComplete)
        {
            // --- BẮT ĐẦU THAY ĐỔI ---
            conversationToStart = questCompletionConversation;

            // THAY VÌ GỌI CompleteQuest(), CHÚNG TA ĐẶT CỜ
            isFinishingQuest = true;
            // --- KẾT THÚC THAY ĐỔI ---
        }
        else // (currentState == QuestManager.QuestState.Completed)
        {
            conversationToStart = postQuestConversation;
        }


        // Bắt đầu hội thoại (Phần này giữ nguyên)
        if (DialogueManager.instance != null && conversationToStart != null)
        {
            DialogueManager.instance.StartConversation(
                conversationToStart,
                playerObject,
                oldManAnimator,
                shouldLockPlayerWhileTalking
            );
        }
        else
        {
            Debug.LogError("Không tìm thấy DialogueManager hoặc Conversation trống!");
            isTalking = false;
        }
    }
    private void SpawnReward()
    {
        if (chestPrefab == null)
        {
            Debug.LogError("CHEST PREFAB bị thiếu! (Trên Inspector của Oldman)");
            return;
        }

        Debug.Log("Đang tạo 5 phần thưởng Chest...");

        // Lấy vị trí trung tâm (chính là vị trí của ông lão)
        Vector3 centerPosition = transform.position;

        // Chạy vòng lặp 5 lần
        for (int i = 0; i < 5; i++)
        {
            // Lấy một vị trí ngẫu nhiên trong vòng tròn bán kính 2.0f
            // Bạn có thể đổi số 2.0f to hơn (để rơi xa hơn) hoặc nhỏ hơn
            Vector2 randomOffset = Random.insideUnitCircle * 2.0f;

            // Tính toán vị trí spawn cuối cùng
            Vector3 spawnPosition = centerPosition + new Vector3(randomOffset.x, randomOffset.y, 0);

            // Tạo ra cái chest
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        }
    }
}