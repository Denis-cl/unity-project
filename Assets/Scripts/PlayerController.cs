using UnityEngine;

/// <summary>
/// Контроллер движения игрока.
/// Вариант B (кинематика): прямое изменение transform.position —
/// выбрано, потому что жанр (2D-аркада на выживание) требует точного,
/// отзывчивого управления без инерции («полный контроль», см. GDD).
///
/// Движение выполняется в Update: обновление каждый кадр синхронно
/// с камерой и отрисовкой. Движение в FixedUpdate (50 Гц) при частоте
/// кадров 60+ давало «биения» — периодические подёргивания картинки.
///
/// Прошивание невозможно: позиция ограничивается границами арены
/// (ClampToBounds) ДО перемещения. Rigidbody2D остаётся на объекте
/// кинематическим — для будущих коллизий (враги, бонусы).
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;   // Скорость перемещения (инспектор)

    private Vector2 moveInput;
    private Bounds movementBounds;                   // Границы арены (из LevelBounds)
    private Collider2D col;                          // Кэш коллайдера (для учёта размера при клампе)

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    /// <summary>Границы задаются извне — скриптом LevelBounds при старте сцены.</summary>
    public void SetBounds(Bounds bounds) => movementBounds = bounds;

    private void Update()
    {
        // Ввод: WASD / стрелки
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = Vector2.ClampMagnitude(moveInput, 1f); // Диагональ не быстрее прямой

        // Новая позиция = текущая + направление * скорость * время кадра
        Vector2 newPos = (Vector2)transform.position + moveSpeed * Time.deltaTime * moveInput;

        // Ограничение границами арены — до применения позиции
        newPos = ClampToBounds(newPos);

        transform.position = newPos;
    }

    /// <summary>
    /// Прижимает позицию к границам арены с учётом половины размера коллайдера,
    /// чтобы персонаж останавливался «корпусом» у стены, а не центром.
    /// </summary>
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