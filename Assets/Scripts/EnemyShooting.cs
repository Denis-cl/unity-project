using UnityEngine;

/// <summary>
/// Стрельба врага по игроку в радиусе ShootRange.
/// Все параметры — из EnemyStats; урон пробрасывается в пулю при создании.
/// </summary>
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private EnemyStats stats;   // Ассет с параметрами
    [SerializeField] private Transform firePoint;

    private Transform player;
    private float nextFireTime;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || stats == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > stats.ShootRange) return;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + stats.FireInterval;

        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        // Урон и скорость пули передаются из EnemyStats в момент выстрела
        bullet.GetComponent<EnemyBullet>().Init(dir, stats.BulletSpeed, stats.BulletDamage);

        Debug.Log("[EnemyShooting] Враг выстрелил!");
    }
}