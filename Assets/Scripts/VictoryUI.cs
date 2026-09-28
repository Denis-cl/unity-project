using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Панель «ПОБЕДА» на весь экран. Подписывается на VictoryManager.OnVictory.
/// Кнопка перезапускает сцену (и возвращает Time.timeScale).
/// </summary>
public class VictoryUI : MonoBehaviour
{
    [SerializeField] private VictoryManager victoryManager;
    [SerializeField] private GameObject victoryPanel;

    private void Awake()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    private void Start()
    {
        if (victoryManager != null) victoryManager.OnVictory += Show;
    }

    private void OnDestroy()
    {
        if (victoryManager != null) victoryManager.OnVictory -= Show;
    }

    private void Show()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Debug.Log("[VictoryUI] Показана панель победы.");
    }

    /// <summary>Кнопка «Играть снова».</summary>
    public void Restart()
    {
        Debug.Log("[VictoryUI] Перезапуск игры...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}