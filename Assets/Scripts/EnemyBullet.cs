using UnityEngine;

/// <summary>
/// Пуля врага. Урон и скорость задаются при выстреле через Init()
/// (из EnemyStats) — сама пуля ничего не знает о балансе.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 4f;

    private Vector2 direction;
    private float speed;
    private int damage;

    public void Init(Vector2 dir, float spd, int dmg)
    {
        direction = dir;
        speed = spd;
        damage = dmg;
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
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);

        Debug.Log("[EnemyBullet] Пуля попала в игрока!");
        Destroy(gameObject);
    }
}