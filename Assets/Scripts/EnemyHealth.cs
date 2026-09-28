using UnityEngine;
using TMPro;

/// <summary>
/// Здоровье врага. При HP = 0 враг исчезает и роняет случайный бонус
/// из списка dropPrefabs (разная ценность: обычная/дорогая монета).
/// Монетка появляется рядом с местом смерти (случайный разброс
/// в радиусе dropSpread, чтобы выглядело «выпадением»).
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private GameObject[] dropPrefabs;   // Возможные дропы
    [SerializeField] private float dropSpread = 0.5f;    // Радиус разброса дропа

    private int currentHealth;
    private TMP_Text healthText;

    private void Awake()
    {
        currentHealth = maxHealth;
        healthText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"[EnemyHealth] Враг получил {damage} урона. HP: {currentHealth}/{maxHealth}");
        UpdateText();

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("[EnemyHealth] Враг уничтожен! Выпал бонус.");

        // Случайный дроп из списка
        if (dropPrefabs != null && dropPrefabs.Length > 0)
        {
            var drop = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
            // Позиция: место смерти + случайное смещение в радиусе dropSpread
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle * dropSpread);
            Instantiate(drop, pos, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void UpdateText()
    {
        if (healthText != null)
            healthText.text = $"{Mathf.Max(currentHealth, 0)}/{maxHealth}";
    }
