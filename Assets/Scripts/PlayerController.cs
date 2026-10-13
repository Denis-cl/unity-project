using UnityEngine;

/// <summary>
/// Контроллер движения игрока (кинематика, Вариант B).
/// Параметры скорости — из ScriptableObject PlayerStats (data-driven).
/// Движение в Update: синхронно с отрисовкой, без "биений" FixedUpdate.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;   // Ассет с параметрами

    private Vector2 moveInput;
    private Bounds movementBounds;
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (stats == null) Debug.LogError("[PlayerController] Не назначен PlayerStats!");
    }

    public void SetBounds(Bounds bounds) => movementBounds = bounds;

    private void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = Vector2.ClampMagnitude(moveInput, 1f); // Диагональ не быстрее прямой

        Vector2 newPos = (Vector2)transform.position + stats.MoveSpeed * Time.deltaTime * moveInput;
        newPos = ClampToBounds(newPos);
        transform.position = newPos;
    }

    /// <summary>Прижимает позицию к границам арены с учётом половины размера коллайдера.</summary>
    private Vector2 ClampToBounds(Vector2 pos)
    {
        if (movementBounds.size == Vector3.zero) return pos;

        Vector2 halfSize = Vector2.zero;
        if (col != null) halfSize = col.bounds.extents;

        pos.x = Mathf.Clamp(pos.x,
            movementBounds.min.x + halfSize.x,
            movementBounds.max.x - halfSize.x);
        pos.y = Mathf.Clamp(pos.y,
            movementBounds.min.y + halfSize.y,
            movementBounds.max.y - halfSize.y);
        return pos;
    }
}