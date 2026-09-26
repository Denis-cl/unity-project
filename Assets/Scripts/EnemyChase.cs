using UnityEngine;

/// <summary>
/// Враг преследует игрока, но держит дистанцию stopDistance:
/// ближе не подходит, стоит и стреляет. Движение через transform
/// (кинематика, как у игрока). При Time.timeScale = 0 движение
/// останавливается само (Time.deltaTime = 0).
/// </summary>
public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stopDistance = 4f;   // Дистанция, с которой враг стоит и стреляет

    private Transform player;

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= stopDistance) return;   // Достаточно близко — стоим, стреляет EnemyShooting

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime);
    }
}
