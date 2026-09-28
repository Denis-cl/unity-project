using UnityEngine;

/// <summary>
/// Здоровье игрока. HP, урон и НЕУЯЗВИМОСТЬ (щит).
/// Неуязвимость — метка времени (invulnerableUntil): удобно выводить
/// остаток секунд в UI. Пока щит активен: урон игнорируется,
/// вокруг игрока горит ореол (shieldVisual).
/// При HP = 0: событие OnDeath, остановка времени, поражение в консоль.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject shieldVisual;   // Ореол (ShieldAura)

    private int currentHealth;
    private bool isDead = false;
    private float invulnerableUntil;
    private bool invulnWasActive = false;

    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;
    public bool IsInvulnerable => Time.time < invulnerableUntil;
    public float InvulnerableTimeLeft => Mathf.Max(0f, invulnerableUntil - Time.time);

    /// <summary>Событие смерти: подписывается GameOverUI.</summary>
    public event System.Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        Debug.Log($"[PlayerHealth] Старт. HP: {currentHealth}/{maxHealth}");
    }

    private void Update()
    {
        // Однократное срабатывание в момент окончания щита
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
            Debug.Log($"[PlayerHealth] Щит поглотил урон: {damage}! HP без изменений: {currentHealth}/{maxHealth}");
            return;
        }

        currentHealth -= damage;
        Debug.Log($"[PlayerHealth] Получен урон: {damage}. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0) Die();
    }

    /// <summary>Неуязвимость на duration секунд (вызывает ShieldPickup).</summary>
    public void SetInvulnerable(float duration)
    {
        invulnerableUntil = Time.time + duration;   // Повторный подбор = перезапуск таймера
        Debug.Log($"[PlayerHealth] НЕУЯЗВИМОСТЬ активна на {duration} сек!");
        if (shieldVisual != null) shieldVisual.SetActive(true);
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
        Time.timeScale = 0f;

        OnDeath?.Invoke();
    }
}