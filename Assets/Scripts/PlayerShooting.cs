using UnityEngine;

/// <summary>
/// Стрельба игрока. ЛКМ — выстрел снарядом в направлении курсора.
/// Пушка (Gun) поворачивается за мышью, снаряд вылетает из её дула (Fire Point).
/// Скорострельность ограничена fireRate (зажатая кнопка ≠ пулемёт).
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;  // Префаб снаряда
    [SerializeField] private Transform gun;                // Пушка (вращается за мышью)
    [SerializeField] private Transform firePoint;          // Дуло (точка вылета снаряда)
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float fireRate = 0.25f;       // Секунд между выстрелами

    private Camera cam;
    private float nextFireTime;

    private void Awake()
    {
        cam = Camera.main;   // Кэшируем один раз 
    }

    private void Update()
    {
        if (cam == null) return;

        // --- Прицеливание: пушка поворачивается к курсору ---
        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mouseWorld - (Vector2)transform.position;
        float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg;
        if (gun != null) gun.rotation = Quaternion.Euler(0, 0, angle);

        // --- Стрельба: ЛКМ с ограничением скорострельности ---
        if (!Input.GetMouseButton(0)) return;    // ЛКМ не зажата — не стреляем
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;
        Fire(toMouse.normalized);
    }

    private void Fire(Vector2 direction)
    {
        var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Init(direction, projectileSpeed);
        Debug.Log("[PlayerShooting] Выстрел!");
    }
}