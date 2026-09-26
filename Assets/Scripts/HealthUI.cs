using UnityEngine;
using TMPro;

/// <summary>
/// Отображение HP игрока над его головой (World Space Canvas, дочерний к Player).
/// Катается вместе с игроком автоматически — канвас привязан к нему иерархией.
/// </summary>
public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text healthText;

    private void Update()
    {
        if (playerHealth == null || healthText == null) return;
        healthText.text = $"{playerHealth.CurrentHealth}/100";
    }
}