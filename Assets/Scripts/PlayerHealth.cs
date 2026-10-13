using UnityEngine;

/// <summary>
/// Здоровье игрока. Максимум HP — из PlayerStats (data-driven),
/// текущее HP — рантайм-состояние здесь (SO не хранит состояние).
/// Неуязвимость — метка времени (удобно выводить остаток в UI).
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;       // Ассет с параметрами
    [SerializeField] private GameObject shieldVisual;

    private int currentHealth;
    private bool isDead = false;
    private float invulnerableUntil;
    private bool invulnWasActive = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => stats.MaxHealth;
    public bool IsDead => isDead;
    public bool IsInvulnerable => Time.time < invulnerableUntil;
    public float InvulnerableTimeLeft => Mathf.Max(0f, invulnerableUntil - Time.time);

    /// <summary>Состояние игрока — enum для читаемости.</summary>
    public enum PlayerState { Alive, Invulnerable, Dead }
    public PlayerState State =>
        isDead ? PlayerState.Dead
        : IsInvulnerable ? PlayerState.Invulnerable
        : PlayerState.Alive;

    public event System.Action OnDeath;

    private void Awake()
    {
        if (stats == null) Debug.LogError("[PlayerHealth] Не назначен PlayerStats!");
        currentHealth = stats.MaxHealth;
    }

    private void Start()
    {
        Debug.Log($"[PlayerHealth] Старт. HP: {currentHealth}/{MaxHealth}. Состояние: {State}");
    }

    private void Update()
    {
        bool active = IsInvulnerable;
        if (invulnWasActive && !active)
        {
            Debug.Log("[PlayerHealth] Неуязвимость закончилась.");
            if (shieldVisual != null) shieldVisual.SetActive(false);
        }
        invulnWasActive = active;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (IsInvulnerable)
        {
            Debug.Log($"[PlayerHealth] Щит поглотил урон: {damage}! HP без изменений: {currentHealth}/{MaxHealth}");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"[PlayerHealth] Получен урон: {damage}. HP: {currentHealth}/{MaxHealth}");

        if (currentHealth <= 0) Die();
    }

    /// <summary>Лечение (используется HealingShrine).</summary>
    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, MaxHealth);
        Debug.Log($"[PlayerHealth] Исцеление: +{amount}. HP: {currentHealth}/{MaxHealth}");
    }

    public void SetInvulnerable(float duration)
    {
        invulnerableUntil = Time.time + duration;
        Debug.Log($"[PlayerHealth] НЕУЯЗВИМОСТЬ активна на {duration} сек!");
        if (shieldVisual != null) shieldVisual.SetActive(true);
    }

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
        Time.timeScale = 0f;
        OnDeath?.Invoke();
    }
}