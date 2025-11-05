using UnityEngine;
using System.Collections;

public class Open : MonoBehaviour
{
    public GameObject[] ItemPrefabs; // Danh sách vật phẩm có thể rơi
    public Sprite closedChest;       // Sprite khi rương đóng
    public Sprite openedChest;       // Sprite khi rương mở
    public Transform dropPoint;      // Nơi vật phẩm xuất hiện (có thể là transform con)
    [HideInInspector] public bool isOpened = false;
    private AudioManager audioManager;

    private SpriteRenderer sr;
    private bool isPlayerNearby = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedChest;

        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (isPlayerNearby && !isOpened && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }

    public void OpenChest() // public để PlayerInteract có thể gọi
    {
        if (isOpened) return;

        if (audioManager != null)
        {
            audioManager.EventChestOpen(); // Phát tiếng mở rương
        }
        else
        {
            Debug.LogWarning("Không tìm thấy AudioManager trong Scene!");
        }

        isOpened = true;
        sr.sprite = openedChest;

        int randomIndex = Random.Range(0, ItemPrefabs.Length);
        GameObject item = Instantiate(ItemPrefabs[randomIndex], dropPoint.position, Quaternion.identity);

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(new Vector2(Random.Range(-1f, 1f), 2f), ForceMode2D.Impulse);
        }
        // Bắt đầu Coroutine biến mất sau 1.5 giây
        StartCoroutine(DisappearAfterDelay(1.5f));
    }

    private IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Tuỳ chọn: hiệu ứng thu nhỏ trước khi biến mất
        float shrinkDuration = 0.3f;
        Vector3 originalScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < shrinkDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsed / shrinkDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Xóa rương khỏi scene
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Nhấn F để mở rương!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
