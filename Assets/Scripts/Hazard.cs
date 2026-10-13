using UnityEngine;

/// <summary>
/// Опасная зона: урон за движение (из GameBalance) + мгновенная смерть
/// за неподвижность stillKillTime секунд.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [SerializeField] private GameBalance balance;   // Ассет с параметрами
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float moveThreshold = 0.01f;

    private float lastTickTime = -999f;
    private bool playerInside = false;
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private Vector2 lastPlayerPos;
    private float stillTime = 0f;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        playerTransform = other.transform;
        playerHealth = other.GetComponent<PlayerHealth>();
        lastPlayerPos = playerTransform.position;
        stillTime = 0f;
        Debug.Log("[Hazard] Игрок вошёл в опасную зону. Двигайся, чтобы выжить!");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        playerTransform = null;
        playerHealth = null;
        stillTime = 0f;
        Debug.Log("[Hazard] Игрок вышел из опасной зоны.");
    }

    private void Update()
    {
        if (!playerInside || playerTransform == null || playerHealth == null) return;

        Vector2 currentPos = playerTransform.position;
        bool isMoving = Vector2.Distance(currentPos, lastPlayerPos) > moveThreshold;

        if (isMoving)
        {
            stillTime = 0f;   // движение сбрасывает таймер неподвижности
            if (Time.time - lastTickTime >= tickInterval)
            {
                lastTickTime = Time.time;
                playerHealth.TakeDamage(balance.HazardDamagePerTick);
            }
        }
        else
        {
            stillTime += Time.deltaTime;
            if (stillTime >= balance.StillKillTime)
            {
                Debug.Log("[Hazard] Игрок стоял неподвижно слишком долго — HP обнулено!");
                playerHealth.Kill();
                stillTime = 0f;
            }
        }

        lastPlayerPos = currentPos;
    }
}