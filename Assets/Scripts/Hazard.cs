using UnityEngine;

/// <summary>
/// Опасная зона (Proximity из GDD).
/// Логика «движение = выживание»:
/// 1) Пока игрок ДВИГАЕТСЯ в зоне — получает периодический урон (damagePerTick).
/// 2) Если игрок СТОИТ НЕПОДВИЖНО stillKillTime секунд — мгновенная смерть (Kill),
///    независимо от текущего HP.
/// Условие неподвижности проверяется каждый кадр: позиция сравнивается с позицией
/// на предыдущем кадре; сдвиг больше moveThreshold считается движением.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    [Header("Урон за движение")]
    [SerializeField] private int damagePerTick = 5;       // Урон за тик при движении
    [SerializeField] private float tickInterval = 0.5f;   // Как часто тикает урон

    [Header("Смерть за неподвижность")]
    [SerializeField] private float stillKillTime = 5f;    // Сколько можно стоять неподвижно
    [SerializeField] private float moveThreshold = 0.01f; // Порог «движения» (ед. / кадр)

    private bool playerInside = false;
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private Vector2 lastPlayerPos;
    private float stillTime = 0f;
    private float lastTickTime = -999f;

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
            // --- ДВИЖЕНИЕ: периодический урон + сброс таймера неподвижности ---
            stillTime = 0f;

            if (Time.time - lastTickTime >= tickInterval)
            {
                lastTickTime = Time.time;
                playerHealth.TakeDamage(damagePerTick);
            }
        }
        else
        {
            // --- НЕПОДВИЖНОСТЬ: накапливаем время ---
            stillTime += Time.deltaTime;

            if (stillTime >= stillKillTime)
            {
                Debug.Log("[Hazard] Игрок стоял неподвижно слишком долго — HP обнулено!");
                playerHealth.Kill();   // Мгновенная смерть при любом HP
            }
        }

        lastPlayerPos = currentPos;
    }
}