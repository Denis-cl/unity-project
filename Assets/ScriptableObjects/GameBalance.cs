using UnityEngine;

/// <summary>
/// Data-driven конфигурация мира: спавн, зоны, условие победы.
/// Ассет: Create → Game → Game Balance.
/// </summary>
[CreateAssetMenu(fileName = "NewGameBalance", menuName = "Game/Game Balance")]
public class GameBalance : ScriptableObject
{
    [Header("Спавн врагов")]
    [SerializeField, Range(5f, 60f), Tooltip("Интервал между подкреплениями (сек). Темп нарастания давления")]
    private float spawnInterval = 15f;

    [Header("Опасная зона")]
    [SerializeField, Min(0), Tooltip("Урон за тик при движении внутри зоны")]
    private int hazardDamagePerTick = 5;
    [SerializeField, Range(1f, 30f), Tooltip("Сколько секунд можно стоять неподвижно до мгновенной смерти")]
    private float stillKillTime = 5f;

    [Header("Победа")]
    [SerializeField, Min(10), Tooltip("Счёт золота для победы")]
    private int scoreToWin = 100;

    public float SpawnInterval => spawnInterval;
    public int HazardDamagePerTick => hazardDamagePerTick;
    public float StillKillTime => stillKillTime;
    public int ScoreToWin => scoreToWin;
}