using UnityEngine;
using TMPro;

/// <summary>
/// Отображение HP игрока над его головой (World Space Canvas, дочерний к Player).
/// Максимум HP читается из PlayerHealth — ничего не захардкожено,
/// значение актуально при любом maxHealth (и при переходе на ScriptableObject).
/// </summary>
public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;  // Здоровье игрока
    [SerializeField] private TMP_Text healthText;        // Текст на канвасе

    private void Update()
    {
        if (playerHealth == null || healthText == null) return;

        // Формат "текущее/максимальное" — максимум из PlayerHealth, а не из строки
        healthText.text = $"{playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
    }
}