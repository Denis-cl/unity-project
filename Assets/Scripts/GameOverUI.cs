using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Меню поражения. Подписывается на событие смерти игрока,
/// показывает панель «ПОРАЖЕНИЕ». Кнопка рестарта перезагружает сцену
/// (и возвращает Time.timeScale в 1).
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;   // Здоровье игрока
    [SerializeField] private GameObject gameOverPanel;    // Панель меню (скрыта до смерти)

    private void Awake()
    {
        // На старте меню скрыто
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        // Подписка на смерть игрока
        if (playerHealth != null) playerHealth.OnDeath += Show;
    }

    private void OnDestroy()
    {
        // Отписка — чтобы не было ошибок при перезагрузке сцены
        if (playerHealth != null) playerHealth.OnDeath -= Show;
    }

    private void Show()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Debug.Log("[GameOverUI] Показано меню поражения.");
    }

    /// <summary>Вызывается кнопкой «Начать заново».</summary>
    public void Restart()
    {
        Debug.Log("[GameOverUI] Перезапуск игры...");
        Time.timeScale = 1f;   // Возвращаем время, иначе новая сцена тоже будет заморожена
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}