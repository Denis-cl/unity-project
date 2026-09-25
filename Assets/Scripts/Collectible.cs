using UnityEngine;

/// <summary>
/// Собираемый предмет (золото/бонус из GDD). 
/// Тип взаимодействия: КОНТАКТ через триггер (OnTriggerEnter2D) —
/// предмет не препятствие, сбор без столкновения.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 10;        // Ценность предмета (очки)

    private static int totalCollected = 0;          // Счётчик собранных (статический — общий для всех)
    private static int totalScore = 0;

    private void Awake()
    {
        // Коллайдер предмета обязан быть триггером
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что вошёл именно игрок (по тегу)
        if (!other.CompareTag("Player")) return;

        totalCollected++;
        totalScore += value;

        // Отладочный вывод состояния (пункт 5 задания)
        Debug.Log($"[Collectible] Собрано золото (+{value}). Всего предметов: {totalCollected}, очков: {totalScore}");

        Destroy(gameObject); // Предмет исчезает 
    }
}