using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Меню поражения. Показывает счёт/рекорд и топ-3, прячет HUD-счёт,
/// по смерти СОХРАНЯЕТ забег через SaveManager.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject hudScore;     // Счёт вверху — прячем
    [SerializeField] private TMP_Text resultText;     // "Золото: X / Рекорд: Y"
    [SerializeField] private TMP_Text topText;        // Топ-3

    private void Awake()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        if (playerHealth != null) playerHealth.OnDeath += Show;
    }

    private void OnDestroy()
    {
        if (playerHealth != null) playerHealth.OnDeath -= Show;
    }

    private void Show()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (hudScore != null) hudScore.SetActive(false);

        int score = Collectible.TotalScore;
        bool newRecord = score > 0 && score >= SaveManager.Instance.HighScore;

        RefreshStats();

        if (newRecord && resultText != null)
            resultText.text += "   — НОВЫЙ РЕКОРД!";

        // Точка сохранения: конец забега. Атомарно, одной записью.
        SaveManager.Instance.SaveRun(score);

        Debug.Log("[GameOverUI] Показано меню поражения.");
    }

    private void RefreshStats()
    {
        if (resultText != null)
            resultText.text = $"Золото: {Collectible.TotalScore}   Рекорд: {SaveManager.Instance.HighScore}";
        if (topText != null)
            topText.text = SaveManager.Instance.GetTopText();
    }

    /// <summary>Кнопка «Сбросить прогресс» на панели.</summary>
    public void ResetProgressClick()
    {
        SaveManager.Instance.ResetProgress();
        RefreshStats();
        Debug.Log("[GameOverUI] Прогресс сброшен игроком.");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}