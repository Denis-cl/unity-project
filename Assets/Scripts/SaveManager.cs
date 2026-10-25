using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// SaveManager: сохранение/загрузка прогресса через PlayerPrefs.
/// Паттерн Singleton — доступ отовсюду: SaveManager.Instance.
///
/// Ключи:
///   "high_score" — рекорд (int)
///   "top_scores" — топ-3 забегов (строка "120,80,50")
///   "runs_played" — количество завершённых забегов (int)
///   "difficulty" — выбранный пресет сложности (string)
///
/// Защита данных:
///  - ключа нет → значения по умолчанию (0 / "Normal" / пустой топ);
///  - значение повреждено (не парсится) → оно пропускается, не роняет игру.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string KEY_HIGH = "high_score";
    private const string KEY_TOP = "top_scores";
    private const string KEY_RUNS = "runs_played";
    private const string KEY_DIFF = "difficulty";

    private const int TOP_SIZE = 3;

    public int HighScore { get; private set; }
    public int RunsPlayed { get; private set; }
    public string Difficulty { get; private set; } = "Normal";

    private readonly List<int> topScores = new List<int>();

    private void Awake()
    {
        Instance = this;
        Collectible.ResetScore();   // ← новый забег начинается с нуля (раньше это делал GameManager)
        LoadAll();
        Debug.Log($"[SaveManager] Загружено. Рекорд: {HighScore}, забегов: {RunsPlayed}, топ: {string.Join(",", topScores)}");
    }

    private void LoadAll()
    {
        // Отсутствие ключа → дефолт (второй аргумент)
        HighScore = PlayerPrefs.GetInt(KEY_HIGH, 0);
        RunsPlayed = PlayerPrefs.GetInt(KEY_RUNS, 0);
        Difficulty = PlayerPrefs.GetString(KEY_DIFF, "Normal");

        // Топ-3: парсинг с защитой от повреждённых данных
        topScores.Clear();
        if (PlayerPrefs.HasKey(KEY_TOP))
        {
            foreach (string part in PlayerPrefs.GetString(KEY_TOP).Split(','))
            {
                if (int.TryParse(part.Trim(), out int value) && value >= 0)
                    topScores.Add(value);
                else
                    Debug.LogWarning($"[SaveManager] Пропущено повреждённое значение топа: '{part}'");
            }
        }
        topScores.Sort((a, b) => b.CompareTo(a));   // по убыванию
    }

    /// <summary>Вызывается при завершении забега (смерть). Пишет всё на диск.</summary>
    public void SaveRun(int runScore)
    {
        RunsPlayed++;
        if (runScore > HighScore) HighScore = runScore;

        topScores.Add(runScore);
        topScores.Sort((a, b) => b.CompareTo(a));
        if (topScores.Count > TOP_SIZE)
            topScores.RemoveRange(TOP_SIZE, topScores.Count - TOP_SIZE);

        PlayerPrefs.SetInt(KEY_HIGH, HighScore);
        PlayerPrefs.SetInt(KEY_RUNS, RunsPlayed);
        PlayerPrefs.SetString(KEY_TOP, string.Join(",", topScores));
        PlayerPrefs.Save();
        Debug.Log($"[SaveManager] Забег сохранён. Рекорд: {HighScore}, всего забегов: {RunsPlayed}");
    }

    /// <summary>Топ-3 строкой для UI.</summary>
    public string GetTopText()
    {
        if (topScores.Count == 0) return "Топ пуст — сыграй первый забег!";
        var sb = new StringBuilder("Топ-3 забега:\n");
        for (int i = 0; i < topScores.Count; i++)
            sb.AppendLine($"{i + 1}. {topScores[i]} золота");
        return sb.ToString();
    }

    /// <summary>Кнопка «Сбросить прогресс».</summary>
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        HighScore = 0;
        RunsPlayed = 0;
        Difficulty = "Normal";
        topScores.Clear();

        Debug.Log("[SaveManager] Прогресс полностью сброшен.");
    }
}
