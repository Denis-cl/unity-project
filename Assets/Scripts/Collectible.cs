using UnityEngine;

/// <summary>
/// Собираемый предмет (золото). Контакт через триггер.
/// Ведёт статический счёт очков и уведомляет игроков
/// (VictoryManager) через событие OnScoreChanged.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 10;

    private static int totalCollected = 0;
    private static int totalScore = 0;

    public static int TotalScore => totalScore;

    /// <summary>Событие изменения счёта (передаёт текущий счёт).</summary>
    public static event System.Action<int> OnScoreChanged;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        totalCollected++;
        totalScore += value;
        Debug.Log($"[Collectible] Собрано золото (+{value}). Всего предметов: {totalCollected}, очков: {totalScore}");

        OnScoreChanged?.Invoke(totalScore);   // Уведомляем VictoryManager

        Destroy(gameObject);
    }

    /// <summary>Сброс счёта (вызывается при старте сцены — static переживает перезагрузку).</summary>
    public static void ResetScore()
    {
        totalCollected = 0;
        totalScore = 0;
    }
}