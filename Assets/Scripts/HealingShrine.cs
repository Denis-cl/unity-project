using System.Collections;
using UnityEngine;

/// <summary>
/// Тип взаимодействия: АКТИВАЦИЯ (кнопка E).
/// Святыня-лекарь: при активации восстанавливает HP ПОСТЕПЕННО —
/// healPerTick каждые tickInterval секунд, пока не наберётся totalHeal.
/// Лечение — «благословение»: продолжается, даже если игрок ушёл.
/// Одноразовая: после активации камень сереет.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HealingShrine : MonoBehaviour
{
    [SerializeField] private int totalHeal = 50;        // Сколько всего восстановит
    [SerializeField] private int healPerTick = 10;      // За один тик
    [SerializeField] private float tickInterval = 1f;   // Каждые сколько секунд
    [SerializeField] private bool oneTimeUse = true;

    private bool playerInRange = false;
    private bool used = false;
    private Coroutine healRoutine;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerHealth = playerObj.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (used || !playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"[HealingShrine] Активировано! Лечение: {healPerTick} HP каждые {tickInterval} сек, всего {totalHeal}.");
            healRoutine = StartCoroutine(HealOverTime());

            if (oneTimeUse)
            {
                used = true;
                GetComponent<SpriteRenderer>().color = Color.gray;   // Визуально "потрашено"
            }
        }
    }

    /// <summary>Постепенное лечение: тики каждую секунду, пока не набран totalHeal.</summary>
    private IEnumerator HealOverTime()
    {
        int healedTotal = 0;

        while (healedTotal < totalHeal)
        {
            yield return new WaitForSeconds(tickInterval);   // пауза между тиками

            if (playerHealth == null || playerHealth.IsDead) yield break;  // игрок умер — стоп

            int amount = Mathf.Min(healPerTick, totalHeal - healedTotal);  // последний тик может быть меньше
            playerHealth.Heal(amount);
            healedTotal += amount;
        }

        Debug.Log($"[HealingShrine] Лечение завершено: +{healedTotal} HP суммарно.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}