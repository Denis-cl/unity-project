using UnityEngine;

/// <summary>
/// Снаряд пушки игрока. Летит по прямой в направлении выстрела,
/// наносит урон врагу, в которого попал (EnemyHealth), и исчезает.
/// Исчезает через lifeTime, чтобы не копиться за пределами арены.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 10;        // Урон (3 попадания = 30 HP врага)
    [SerializeField] private float lifeTime = 3f;

    private Vector2 direction;
    private float speed;

    /// <summary>Направление и скорость задаёт PlayerShooting при выстреле.</summary>
    public void Init(Vector2 dir, float spd)
    {
        direction = dir;
        speed = spd;
        Destroy(gameObject, lifeTime);
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
        if (other.CompareTag("Player")) return;   // В себя не стреляем

        var enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            Debug.Log("[Projectile] Попадание по врагу!");
            Destroy(gameObject);
        }
    }
}