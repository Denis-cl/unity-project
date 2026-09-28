using UnityEngine;

/// <summary>
/// Условие победы: счёт золота достиг scoreToWin.
/// Подписывается на событие Collectible.OnScoreChanged.
/// При победе: лог в консоль, остановка времени, событие OnVictory
/// (подписывается VictoryUI — показывает панель «ПОБЕДА»).
/// </summary>
public class VictoryManager : MonoBehaviour
{
    [SerializeField] private int scoreToWin = 100;

    private bool hasWon = false;

    /// <summary>Событие победы: подписывается VictoryUI.</summary>
    public event System.Action OnVictory;

    private void Awake()
    {
        // Static-поля переживают перезагрузку сцены — новая попытка начинается с нуля
        Collectible.ResetScore();
    }

    private void OnEnable()
    {
        Collectible.OnScoreChanged += CheckScore;
    }

    private void OnDisable()
    {
        Collectible.OnScoreChanged -= CheckScore;
    }

    private void CheckScore(int score)
    {
        if (hasWon || score < scoreToWin) return;

        hasWon = true;
        Debug.Log($"=== ПОБЕДА === Счёт {score} достигнут! Цель выполнена.");
        Time.timeScale = 0f;
        OnVictory?.Invoke();
    }
}