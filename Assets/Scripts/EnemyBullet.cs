using UnityEngine;

/// <summary>
/// Пуля врага. Летит по прямой, наносит урон ТОЛЬКО игроку.
/// Исчезает по истечении lifeTime (стен в арене нет — пуля просто улетает).
/// Игрок может уклоняться: пуля не самонаводится, направление фиксируется
/// в момент выстрела.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifeTime = 4f;

    private Vector2 direction;
    private float speed;

    /// <summary>Направление и скорость задаёт EnemyShooting в момент выстрела.</summary>
    public void Init(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
        Destroy(gameObject, lifeTime);   // Страховка от скопления пуль
    }

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);

        Debug.Log("[EnemyBullet] Пуля попала в игрока!");
        Destroy(gameObject);
    }
}