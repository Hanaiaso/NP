using UnityEngine;
using TMPro; // Dùng cho TextMeshPro
using UnityEngine.UI; // Dùng cho Image
using System.Collections.Generic; // Dùng cho Queue

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elements")]
    public GameObject dialoguePanel;
    public Image portraitImage;      // Kéo PortraitImage vào đây
    public TMP_Text nameText;
    public TMP_Text lineText;

    [Header("Settings")]
    public KeyCode continueKey = KeyCode.F; // Đổi thành phím F!
    public string playerMovementScriptName = "Player"; // !! GHI ĐÚNG TÊN script di chuyển

    private Queue<DialogueLine> dialogueQueue;
    private GameObject currentPlayer;
    private Animator currentNpcAnimator;
    private bool shouldLockPlayer;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        dialogueQueue = new Queue<DialogueLine>();
    }

    void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Update()
    {
        // Lắng nghe phím F KHI hội thoại đang diễn ra
        if (dialoguePanel.activeInHierarchy && Input.GetKeyDown(continueKey))
        {
            DisplayNextLine();
        }
    }

    public void StartConversation(Conversation conversation, GameObject player, Animator npcAnimator, bool lockPlayer)
    {
        dialoguePanel.SetActive(true);

        currentPlayer = player;
        currentNpcAnimator = npcAnimator;
        shouldLockPlayer = lockPlayer;

        // 1. Khóa người chơi
        if (shouldLockPlayer && currentPlayer != null)
        {
            var movementScript = currentPlayer.GetComponent(playerMovementScriptName) as MonoBehaviour;
            if (movementScript != null)
                movementScript.enabled = false;
        }

        // 2. Bật animation "Talk" của NPC (nếu có)
        if (currentNpcAnimator != null)
            currentNpcAnimator.SetBool("Talk", true);

        // 3. Nạp lời thoại vào hàng đợi
        dialogueQueue.Clear();
        foreach (DialogueLine line in conversation.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        // 4. Hiển thị câu đầu tiên
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();

        // CẬP NHẬT GIAO DIỆN
        nameText.text = currentLine.speakerName;
        lineText.text = currentLine.text;

        // Cập nhật ảnh (quan trọng!)
        if (currentLine.portrait != null)
        {
            portraitImage.sprite = currentLine.portrait;
            portraitImage.gameObject.SetActive(true); // Bật nó lên
        }
        else
        {
            portraitImage.gameObject.SetActive(false); // Ẩn nếu không có ảnh
        }
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        // Mở khóa người chơi
        if (shouldLockPlayer && currentPlayer != null)
        {
            var movementScript = currentPlayer.GetComponent(playerMovementScriptName) as MonoBehaviour;
            if (movementScript != null)
                movementScript.enabled = true;
        }

        // Tắt animation "Talk"
        if (currentNpcAnimator != null)
            currentNpcAnimator.SetBool("Talk", false);

        currentPlayer = null;
        currentNpcAnimator = null;
    }
}