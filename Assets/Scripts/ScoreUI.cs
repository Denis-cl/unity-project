using UnityEngine;
using TMPro;

/// <summary>
/// HUD: золото текущего забега + рекорд (из SaveManager).
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void OnEnable() { Collectible.OnScoreChanged += UpdateText; }
    private void OnDisable() { Collectible.OnScoreChanged -= UpdateText; }

    private void Start()
    {
        UpdateText(Collectible.TotalScore);
    }

    private void UpdateText(int score)
    {
        if (scoreText == null) return;
        int record = SaveManager.Instance != null ? SaveManager.Instance.HighScore : 0;
        scoreText.text = $"Золото: {score}   Рекорд: {record}";
    }
}