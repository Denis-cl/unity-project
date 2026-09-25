using UnityEngine;

/// <summary>
/// Опасная зона (Proximity из GDD: «пока игрок находится внутри —
/// получает периодический урон»). Срабатывает через OnTriggerStay2D:
/// событие каждый физический кадр, пока объект внутри зоны.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 5;
    [SerializeField] private float tickInterval = 0.5f; // Как часто тикает урон

    private float lastTickTime = -999f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time - lastTickTime < tickInterval) return;

        lastTickTime = Time.time;
        Debug.Log("[Hazard] Игрок в опасной зоне — периодический урон!");

        var health = other.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damagePerTick);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("[Hazard] Игрок вышел из опасной зоны.");
    }
}