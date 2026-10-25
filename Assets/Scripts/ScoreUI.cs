using UnityEngine;
using TMPro;

/// <summary>
/// Счёт золота на экране (вечный режим).
/// Подписывается на статическое событие Collectible.OnScoreChanged —
/// текст обновляется только при сборе монеты (без работы каждый кадр).
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable()
    {
        // Static-событие живёт между сценами — подписываем/отписываем аккуратно
        Collectible.OnScoreChanged += UpdateText;
    }

    private void OnDisable()
    {
        Collectible.OnScoreChanged -= UpdateText;
    }

    private void Start()
    {
        UpdateText(Collectible.TotalScore);   // начальное значение
    }

    private void UpdateText(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Золото: {score}";
    }
}