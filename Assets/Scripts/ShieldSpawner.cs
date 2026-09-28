using UnityEngine;

/// <summary>
/// Спавнит щит через spawnDelay секунд после старта в случайной
/// точке арены. Координаты берутся из коллайдера LevelBounds —
/// работает при любом размере карты (в т.ч. после расширения арены).
/// </summary>
public class ShieldSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab;   // Префаб щита
    [SerializeField] private BoxCollider2D arena;       // Коллайдер LevelBounds
    [SerializeField] private float spawnDelay = 10f;    // Секунд до появления
    [SerializeField] private float edgeMargin = 1.5f;   // Отступ от стен

    private bool hasSpawned = false;

    private void Update()
    {
        if (hasSpawned) return;
        if (Time.timeSinceLevelLoad < spawnDelay) return;

        hasSpawned = true;
        SpawnShield();
    }

    private void SpawnShield()
    {
        if (shieldPrefab == null || arena == null) return;

        Bounds b = arena.bounds;
        Vector2 pos = new Vector2(
            Random.Range(b.min.x + edgeMargin, b.max.x - edgeMargin),
            Random.Range(b.min.y + edgeMargin, b.max.y - edgeMargin));

        Instantiate(shieldPrefab, pos, Quaternion.identity);
        Debug.Log($"[ShieldSpawner] Щит появился в точке ({pos.x:F1}; {pos.y:F1})!");
    }
}