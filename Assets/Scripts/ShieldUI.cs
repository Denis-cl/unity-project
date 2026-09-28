using UnityEngine;
using TMPro;

/// <summary>
/// Таймер обратного отсчёта неуязвимости.
/// Показывает «ЩИТ: X.X» под полоской HP, пока щит активен.
/// </summary>
public class ShieldUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TMP_Text shieldText;

    private void Update()
    {
        if (playerHealth == null || shieldText == null) return;

        if (playerHealth.IsInvulnerable)
        {
            shieldText.gameObject.SetActive(true);
            shieldText.text = $"ЩИТ: {playerHealth.InvulnerableTimeLeft:F1}";
        }
        else
        {
            shieldText.gameObject.SetActive(false);
        }
    }
}