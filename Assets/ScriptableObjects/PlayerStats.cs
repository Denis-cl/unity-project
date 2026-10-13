using UnityEngine;

/// <summary>
/// Data-driven конфигурация игрока. Ассет создаётся через
/// меню: Assets → Create → Game → Player Stats.
/// Только данные — рантайм-состояние (currentHealth) остаётся в PlayerHealth.
/// </summary>
[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Здоровье")]
    [SerializeField, Min(1), Tooltip("Максимальное HP. Соотношение с уроном врага задаёт 'право на ошибку'")]
    private int maxHealth = 100;

    [Header("Движение")]
    [SerializeField, Range(1f, 15f), Tooltip("Скорость перемещения игрока (ед./сек). Связана с размером арены")]
    private float moveSpeed = 8f;

    [Header("Бой")]
    [SerializeField, Range(1f, 30f), Tooltip("Скорость снарядов игрока. Должна быть заметно выше скорости врагов")]
    private float projectileSpeed = 12f;
    [SerializeField, Range(0.05f, 2f), Tooltip("Минимальный интервал между выстрелами (сек)")]
    private float fireRate = 0.25f;

    public int MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float ProjectileSpeed => projectileSpeed;
    public float FireRate => fireRate;
}
