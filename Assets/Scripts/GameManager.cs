using UnityEngine;

/// <summary>
/// Менеджер забега. Пока делает одно: сбрасывает статический счёт
/// при загрузке сцены (статика переживает перезагрузку — без сброса
/// после рестарта счёт продолжился бы).
/// В практике 3 вырастет в SaveManager: запись/чтение рекорда.
/// </summary>
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Collectible.ResetScore();
        Debug.Log("[GameManager] Новый забег. Счёт сброшен.");
    }
}