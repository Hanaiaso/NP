using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OldManInteraction : MonoBehaviour
{
    [Header("Indicator")]
    public GameObject interactCanvas;    // Cái "Press E to talk"
    public KeyCode interactKey = KeyCode.E; // Phím E để BẮT ĐẦU

    [Header("Dialogue Data")]
    public Conversation conversation; // Nơi bạn nhập hội thoại
    private Animator oldManAnimator;

    [Header("Optional")]
    public bool shouldLockPlayerWhileTalking = true;
    public string playerTag = "Player";

    private bool playerInRange = false;
    private GameObject playerObject;
    private bool isTalking = false;

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
        // 1. Nếu player trong tầm, chưa nói, và ấn E
        if (playerInRange && Input.GetKeyDown(interactKey) && !isTalking)
        {
            StartDialogue();
        }

        // 2. Reset cờ 'isTalking' khi hội thoại kết thúc
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

    private void StartDialogue()
    {
        isTalking = true;
        if (interactCanvas != null)
            interactCanvas.SetActive(false);

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.StartConversation(
                conversation,
                playerObject,
                oldManAnimator,
                shouldLockPlayerWhileTalking
            );
        }
    }
}