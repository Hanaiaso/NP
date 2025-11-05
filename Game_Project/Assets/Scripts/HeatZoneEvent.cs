using UnityEngine;
using System.Collections;

public class HeatZoneEvent : MonoBehaviour
{
    [Header("Thiết lập vùng nóng")]
    public float duration = 10f;           // Thời gian tồn tại vùng nóng
    public float damagePerSecond = 10f;     // Sát thương mỗi giây
    public SpriteRenderer heatVisual;      // Hiệu ứng vùng nóng (Sprite hoặc vòng lửa)
    public float radius = 2f;              // Bán kính vùng nóng (để dễ điều chỉnh)
    private float damageTimer = 0f;
    private bool active = false;

    // 👉 Hàm này sẽ được gọi từ TestNightfall (hoặc bất kỳ script nào)
    public IEnumerator StartHeatZone()
    {
        // 1️⃣ Tìm Player trong Scene
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("❌ Không tìm thấy Player trong scene!");
            yield break;
        }

        // 2️⃣ Đặt HeatZone ngay tại vị trí Player
        transform.position = player.position;

        // 3️⃣ Kích hoạt vùng nóng
        active = true;
        Debug.Log($"🔥 Heat Zone xuất hiện tại {transform.position}");

        // 4️⃣ Bật hiệu ứng hình ảnh nếu có
        if (heatVisual != null)
        {
            Color c = heatVisual.color;
            c.a = 0.8f;
            heatVisual.color = c;

            // Scale để vùng nóng to vừa phải
            heatVisual.transform.localScale = new Vector3(radius * 2, radius * 2, 1);
        }

        // 5️⃣ Tồn tại trong "duration" giây
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        active = false;
        if (heatVisual != null)
        {
            Color c = heatVisual.color;
            c.a = 0f;
            heatVisual.color = c;
        }

        Debug.Log("🔥 Heat Zone kết thúc!");
        Destroy(gameObject); // Xóa đối tượng
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!active) return;

        if (!active) return;

        damageTimer += Time.deltaTime;
        if (damageTimer >= 1f)
        {
            damageTimer = 0f; // reset mỗi giây

            if (other.CompareTag("Player"))
            {
                Player p = other.GetComponent<Player>();
                if (p != null)
                {
                    p.TakeDamege((int)damagePerSecond);
                    Debug.Log("🔥 Gây sát thương nguyên cho Player");
                }
            }

            if (other.CompareTag("Enemy"))
            {
                Enemy e = other.GetComponent<Enemy>();
                if (e != null)
                {
                    e.TakeDamege((int)damagePerSecond);
                    Debug.Log($"🔥 Gây sát thương nguyên cho Enemy {e.name}");
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}