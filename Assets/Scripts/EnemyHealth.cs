using UnityEngine;
using TMPro;

/// <summary>
/// HP врага (максимум — из EnemyStats) + дроп монеток.
/// Полоска HP над головой ищется автоматически.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;        // Ассет с параметрами
    [SerializeField] private GameObject[] dropPrefabs;
    [SerializeField] private float dropSpread = 0.5f;

    private int currentHealth;
    private TMP_Text healthText;

    private void Awake()
    {
        currentHealth = stats.MaxHealth;
        healthText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"[EnemyHealth] Враг получил {damage} урона. HP: {currentHealth}/{stats.MaxHealth}");
        UpdateText();

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("[EnemyHealth] Враг уничтожен! Выпал бонус.");

        if (dropPrefabs != null && dropPrefabs.Length > 0)
        {
            var drop = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle * dropSpread);
            Instantiate(drop, pos, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void UpdateText()
    {
        if (healthText != null)
            healthText.text = $"{Mathf.Max(currentHealth, 0)}/{stats.MaxHealth}";
    }
}