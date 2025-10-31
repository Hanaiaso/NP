using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SandstormEvent : MonoBehaviour
{
    public ParticleSystem sandstormEffect;  // Hiệu ứng bụi bay
    public Image fogOverlay;                // Ảnh mờ phủ màn hình (UI Image)
    public float duration = 25f;
    public float slowMultiplier = 0.6f;

    private Player player;

    private void Awake()
    {
        // 🔍 Tự động tìm Player và EnemySpawner trong Scene
        player = FindObjectOfType<Player>();
    }

    public IEnumerator StartSandstorm()
    {
        Debug.Log("🏜️ Sandstorm Begins!");
        sandstormEffect.Play();

        // ⚙️ Giảm tốc độ nếu tìm thấy đối tượng
        if (player != null)
            typeof(Player)
                .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(player, (float)typeof(Player)
                .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(player) * slowMultiplier);

        // 💨 Làm mờ vàng dần
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fogOverlay.color = new Color(1f, 0.9f, 0.7f, Mathf.Lerp(0, 0.4f, t));
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        sandstormEffect.Stop();

        // 💨 Làm trong trở lại
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fogOverlay.color = new Color(1f, 0.9f, 0.7f, Mathf.Lerp(0.4f, 0, t));
            yield return null;
        }

        // ✅ Phục hồi tốc độ Player
        if (player != null)
            typeof(Player)
                .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(player, (float)typeof(Player)
                .GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(player) / slowMultiplier);

        Debug.Log("🌤 Sandstorm Ends!");
    }
}
