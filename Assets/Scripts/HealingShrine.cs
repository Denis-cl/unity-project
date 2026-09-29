using UnityEngine;

/// <summary>
/// Тип взаимодействия: АКТИВАЦИЯ (нажатие кнопки).
/// Стоячий камень-лекарь: игрок входит в радиус, нажимает E —
/// HP восстанавливается. Одноразовый (или с перезарядкой).
/// </summary>
public class HealingShrine : MonoBehaviour
{
    [SerializeField] private int healAmount = 50;
    [SerializeField] private float useRadius = 2f;
    [SerializeField] private bool oneTimeUse = true;

    private bool playerInRange = false;
    private bool used = false;

    private void Update()
    {
        if (used || !playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            var health = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.Heal(healAmount);
                Debug.Log($"[HealingShrine] Активировано! Игрок восстановил {healAmount} HP.");
            }
            if (oneTimeUse)
            {
                used = true;
                GetComponent<SpriteRenderer>().color = Color.gray;  // Визуально "потрашено"
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}