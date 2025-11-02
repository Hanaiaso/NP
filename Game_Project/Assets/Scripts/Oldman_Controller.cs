using UnityEngine;

public class OldManController : MonoBehaviour
{
    private Animator animator;
    private bool playerNear = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nếu player ở gần và nhấn F
        if (playerNear && Input.GetKeyDown(KeyCode.F))
        {
            // Nếu đang idle => bắt đầu nói
            if (!animator.GetBool("isTalking"))
            {
                animator.SetBool("isTalking", true);
                Debug.Log("Ông lão bắt đầu nói chuyện");
            }
            // Nếu đang nói => dừng nói
            else
            {
                animator.SetBool("isTalking", false);
                Debug.Log("Ông lão dừng nói chuyện");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Player lại gần ông lão");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            animator.SetBool("isTalking", false); // trở lại MovingStand
            Debug.Log("Player rời khỏi ông lão");
        }
    }
}
