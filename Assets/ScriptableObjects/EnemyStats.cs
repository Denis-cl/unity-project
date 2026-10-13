using UnityEngine;

/// <summary>
/// Data-driven конфигурация врага. Ассет: Create → Game → Enemy Stats.
/// Один ассет = тип врага; варианты (Easy/Normal/Hard) — разные ассеты.
/// </summary>
[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Game/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Здоровье")]
    [SerializeField, Min(1)] private int maxHealth = 30;

    [Header("Преследование")]
    [SerializeField, Range(0.5f, 10f), Tooltip("Скорость врага. Медленнее игрока — иначе уклонение невозможно")]
    private float moveSpeed = 2f;
    [SerializeField, Range(0.5f, 10f), Tooltip("Дистанция, на которой враг останавливается и стреляет")]
    private float stopDistance = 4f;

    [Header("Стрельба")]
    [SerializeField, Range(1f, 30f), Tooltip("Радиус, в котором враг открывает огонь")]
    private float shootRange = 8f;
    [SerializeField, Range(0.1f, 5f), Tooltip("Интервал между выстрелами (сек)")]
    private float fireInterval = 1.5f;
    [SerializeField, Range(1f, 20f), Tooltip("Скорость пули. Должна быть ниже скорости игрока — иначе уклониться нельзя")]
    private float bulletSpeed = 6f;
    [SerializeField, Min(0), Tooltip("Урон пули. Связан с MaxHealth игрока (соотношение = 'право на ошибку')")]
    private int bulletDamage = 10;

    public int MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float StopDistance => stopDistance;
    public float ShootRange => shootRange;
    public float FireInterval => fireInterval;
    public float BulletSpeed => bulletSpeed;
    public int BulletDamage => bulletDamage;
}