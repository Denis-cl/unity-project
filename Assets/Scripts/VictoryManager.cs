using UnityEngine;

/// <summary>
/// Условие победы: счёт золота >= ScoreToWin (из GameBalance).
/// </summary>
public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameBalance balance;   // Ассет с параметрами

    private bool hasWon = false;

    public event System.Action OnVictory;

    private void Awake()
    {
        // Static-поля переживают перезагрузку сцены — новая попытка с нуля
        Collectible.ResetScore();
    }

    private void OnEnable() { Collectible.OnScoreChanged += CheckScore; }
    private void OnDisable() { Collectible.OnScoreChanged -= CheckScore; }

    private void CheckScore(int score)
    {
        if (hasWon || score < balance.ScoreToWin) return;

        hasWon = true;
        Debug.Log($"=== ПОБЕДА === Счёт {score} достигнут! Цель выполнена.");
        Time.timeScale = 0f;
        OnVictory?.Invoke();
    }
}