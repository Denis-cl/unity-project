using UnityEngine;

/// <summary>
/// Стрельба врага. Как только игрок оказывается в радиусе shootRange,
/// враг с периодичностью fireInterval выпускает пулю по направлению
/// к ТЕКУЩЕЙ позиции игрока (пуля не самонаводится — можно уворачиваться).
/// </summary>
public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;    // Префаб EnemyBullet
    [SerializeField] private Transform firePoint;        // Точка вылета пули
    [SerializeField] private float shootRange = 8f;      // Радиус «видимости»
    [SerializeField] private float fireInterval = 1.5f;  // Периодичность стрельбы
    [SerializeField] private float bulletSpeed = 6f;     // Скорость пули

    private Transform player;
    private float nextFireTime;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > shootRange) return;              // Игрок далеко — не стреляем
        if (Time.time < nextFireTime) return;       // Кулдаун

        nextFireTime = Time.time + fireInterval;

        Vector2 dir = ((Vector2)player.position - (Vector2)firePoint.position).normalized;
        var bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().Init(dir, bulletSpeed);

        Debug.Log("[EnemyShooting] Враг выстрелил!");
    }
}