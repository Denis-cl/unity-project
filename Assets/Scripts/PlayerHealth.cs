using UnityEngine;

/// <summary>
/// Здоровье игрока. Единственная ответственность — HP и получение урона.
/// При HP = 0: флаг isDead, событие OnDeath (подписывается UI),
/// остановка времени и запись поражения в консоль.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    /// <summary>Событие смерти: подписывается GameOverUI, чтобы показать меню.</summary>
    public event System.Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        Debug.Log($"[PlayerHealth] Старт. HP: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"[PlayerHealth] Получен урон: {damage}. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0) Die();
    }

    /// <summary>Мгновенная смерть (независимо от текущего HP).</summary>
    public void Kill()
    {
        if (isDead) return;
        Debug.Log("[PlayerHealth] Мгновенная смерть (Kill).");
        Die();
    }

    private void Die()
    {
        currentHealth = 0;
        isDead = true;
        Debug.Log("=== ПОРАЖЕНИЕ === HP достигло нуля. Игра остановлена.");
        Time.timeScale = 0f;   // Вся игра замирает, но UI продолжает работать

        OnDeath?.Invoke();     // Уведомляем игроков (меню поражения)
    }
}