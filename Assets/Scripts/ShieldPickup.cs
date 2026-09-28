using UnityEngine;

/// <summary>
/// Подбираемый щит. Пульсирует (заметность вместо свечения),
/// при касании игрока включает неуязвимость на invulnerabilityDuration
/// секунд и исчезает.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ShieldPickup : MonoBehaviour
{
    [SerializeField] private float invulnerabilityDuration = 5f;
    [SerializeField] private float pulseSpeed = 4f;
    [SerializeField] private float pulseAmount = 0.15f;

    private Vector3 baseScale;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        baseScale = transform.localScale;
    }

    private void Update()
    {
        // Пульсация: размер колеблется ±15% от базового
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * pulse;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.SetInvulnerable(invulnerabilityDuration);
            Debug.Log($"[ShieldPickup] Щит подобран! Неуязвимость на {invulnerabilityDuration} сек.");
        }

        Destroy(gameObject);
    }
}