using UnityEngine;

/// <summary>
/// Враг. Тип взаимодействия: КОНТАКТ — при касании игрока наносит урон.
/// Используется триггер-коллайдер
/// корабль не должен «упираться» во врага, а должен получать урон при касании.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackCooldown = 1f; // Чтобы нельзя было снять весь HP за секунду

    private float lastAttackTime = -999f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryHitPlayer(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // Пока игрок «внутри» врага — бьём с кулдауном
        TryHitPlayer(other);
    }

    private void TryHitPlayer(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        Debug.Log("[Enemy] Контакт с врагом!");

        // Наносим урон через компонент здоровья (а не меняем HP напрямую — SRP)
        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);
    }
}