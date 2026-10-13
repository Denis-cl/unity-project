using UnityEngine;

/// <summary>
/// Преследование игрока с дистанцией остановки. Параметры — из EnemyStats.
/// </summary>
public class EnemyChase : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;   // Ассет с параметрами

    private Transform player;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null || stats == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= stats.StopDistance) return;   // стоим и стреляем

        transform.position = Vector2.MoveTowards(
            transform.position, player.position, stats.MoveSpeed * Time.deltaTime);
    }
}