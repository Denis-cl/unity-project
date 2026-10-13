using UnityEngine;

/// <summary>
/// Стрельба игрока: ПРОБЕЛ — выстрел в направлении курсора.
/// Скорость снаряда и скорострельность — из PlayerStats (data-driven).
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private PlayerStats stats;   // Ассет с параметрами
    [SerializeField] private Transform gun;
    [SerializeField] private Transform firePoint;

    private Camera cam;
    private float nextFireTime;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null || stats == null) return;

        // Прицеливание: пушка поворачивается к курсору
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorld - (Vector2)transform.position;
        float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        if (gun != null) gun.rotation = Quaternion.Euler(0, 0, angle);

        if (!Input.GetKey(KeyCode.Space)) return;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + stats.FireRate;
        Fire(toMouse.normalized);
    }

    private void Fire(Vector2 direction)
    {
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(direction, stats.ProjectileSpeed);
        Debug.Log("[PlayerShooting] Выстрел!");
    }
}