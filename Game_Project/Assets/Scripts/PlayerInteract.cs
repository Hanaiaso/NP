using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 1f;
    public LayerMask interactableLayer;
    public TextMeshProUGUI interactText; // Gắn text “Nhấn F để mở”
    private Open chestInRange;
    private bool canInteract = false;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForChest();

        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            chestInRange.OpenChest();
            interactText.gameObject.SetActive(false);
        }
    }

    void CheckForChest()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange, interactableLayer);

        if (hits.Length > 0)
        {
            chestInRange = hits[0].GetComponent<Open>(); // ← Sửa lại đúng kiểu script của rương

            if (chestInRange != null && !chestInRange.isOpened)
            {
                canInteract = true;
                if (interactText != null)
                    interactText.gameObject.SetActive(true);
                return;
            }
        }

        // Nếu không có rương hoặc rương đã mở
        canInteract = false;
        chestInRange = null;
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
