using UnityEngine;

/// <summary>
/// Зона спавна врагов.Одноразовая: после срабатывания
/// отключается (флаг hasTriggered).
/// </summary>
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;   // Префаб врага
    [SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(6, 4);

    private bool hasTriggered = false;   // Флаг состояния

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        Debug.Log($"[SpawnZone] Игрок вошёл в зону — спавн волны: {enemiesToSpawn} врагов!");

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f));
            Instantiate(enemyPrefab, (Vector2)transform.position + randomOffset, Quaternion.identity);
        }

        // Одноразовая зона (для прототипа). Убираем визуал и коллайдер,
        // чтобы зона больше не срабатывала
        GetComponent<Collider2D>().enabled = false;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }
}