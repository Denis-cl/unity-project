using UnityEngine;

/// <summary>
/// Здоровье игрока. Единственная ответственность — HP и получение урона.
/// Движение живёт в PlayerController, сбор предметов — в Collectible (SRP).
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;   // Флаг состояния (для отладочного вывода)

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Отладочный вывод начального состояния (пункт 5 задания)
        Debug.Log($"[PlayerHealth] Старт. HP: {currentHealth}/{maxHealth}");
    }

    /// <summary>Публичный метод урона — вызывают Enemy и Hazard.</summary>
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"[PlayerHealth] Получен урон: {damage}. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            Debug.Log("[PlayerHealth] Игрок погиб. Game Over (прототип: просто останавливаем ввод)");
            enabled = false; // Отключаем компонент: игрок больше не управляется
        }
    }
}