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

    private Animator oldManAnimator;

    [Header("Optional")]
    public bool shouldLockPlayerWhileTalking = true;
    public string playerTag = "Player";

    private bool playerInRange = false;
    private GameObject playerObject;
    private bool isTalking = false;

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
        if (playerInRange && Input.GetKeyDown(interactKey) && !isTalking)
        {
            StartDialogue();
        }

        if (isTalking &&
            DialogueManager.instance != null &&
            !DialogueManager.instance.dialoguePanel.activeInHierarchy)
        {
            isTalking = false;
            if (playerInRange && interactCanvas != null)
                interactCanvas.SetActive(true);
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
            // Kích hoạt quest ngay khi bắt đầu nói chuyện
            QuestManager.instance.StartQuest();
        }
        else if (currentState == QuestManager.QuestState.InProgress)
        {
            conversationToStart = questInProgressConversation;
        }
        else if (currentState == QuestManager.QuestState.ReadyToComplete)
        {
            conversationToStart = questCompletionConversation;
            // HOÀN THÀNH QUEST NGAY KHI NÓI CHUYỆN
            QuestManager.instance.CompleteQuest();
        }
        else // (currentState == QuestManager.QuestState.Completed)
        {
            conversationToStart = postQuestConversation;
        }


        // Bắt đầu hội thoại
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
}