using UnityEngine;

/// <summary>
/// Спавн врагов: стартовая волна при входе + бесконечный таймер.
/// Интервал подкреплений — из GameBalance (data-driven).
/// </summary>
public class SpawnZone : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameBalance balance;   // Ассет с параметрами
    [SerializeField] private int enemiesToSpawn = 3;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(6, 4);

    private bool hasTriggered = false;
    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + balance.SpawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + balance.SpawnInterval;
            SpawnEnemy();
            Debug.Log("[SpawnZone] Таймер: +1 враг.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;
        Debug.Log($"[SpawnZone] Игрок вошёл — стартовая волна: {enemiesToSpawn} врагов!");

        for (int i = 0; i < enemiesToSpawn; i++) SpawnEnemy();

        GetComponent<Collider2D>().enabled = false;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector2 randomOffset = new Vector2(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f));
        Instantiate(enemyPrefab, (Vector2)transform.position + randomOffset, Quaternion.identity);
    }
}