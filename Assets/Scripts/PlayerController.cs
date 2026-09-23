using UnityEngine;

/// <summary>
/// Контроллер движения игрока.
/// Вариант B (кинематика): Rigidbody2D в режиме Kinematic + MovePosition —
/// выбран, потому что жанр (2D-аркада на выживание) требует точного,
/// отзывчивого управления без инерции: смерть должна восприниматься
/// как ошибка игрока, а не как «непослушное» управление.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;   // Скорость перемещения (настраивается в инспекторе)

    private Rigidbody2D rb;
    private Vector2 moveInput;                        // Вектор ввода (-1..1 по осям)
    private Bounds movementBounds;                    // Границы арены (передаются из LevelBounds)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;      // Физика не двигает объект — управляем только кодом (точное аркадное управление)
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Непрерывная проверка коллизий: объект не «прошивает» стены даже на высокой скорости
    }

    /// <summary>Границы арены задаются извне — скриптом LevelBounds при старте сцены.</summary>
    public void SetBounds(Bounds bounds) => movementBounds = bounds;

    private void Update()
    {
        // Чтение ввода: WASD или стрелки (GetAxisRaw — без сглаживания, отзыв мгновенный)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Нормализация: при зажатых двух клавишах скорость по диагонали
        // не превышает скорость по прямой (иначе диагональ была бы в √2 раз быстрее)
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void FixedUpdate()
    {
        // Движение через MovePosition: физически корректное перемещение кинематического тела,
        // в отличие от прямой записи transform.position учитывает коллизии
        Vector2 newPos = rb.position + moveSpeed * Time.fixedDeltaTime * moveInput;

        // Ограничение границами уровня: персонаж физически не может покинуть арену
        newPos = ClampToBounds(newPos);

        rb.MovePosition(newPos);
    }

    /// <summary>
    /// Прижимает позицию к границам арены. Учитывает половину размера коллайдера,
    /// чтобы персонаж останавливался «корпусом» у стены, а не центром.
    /// </summary>
    private Vector2 ClampToBounds(Vector2 pos)
    {
        // Если границы ещё не переданы — двигаемся без ограничений
        if (movementBounds.size == Vector3.zero) return pos;

        Vector2 halfSize = Vector2.zero;
        var col = GetComponent<Collider2D>();
        if (col != null) halfSize = col.bounds.extents; // Половина ширины/высоты коллайдера

        pos.x = Mathf.Clamp(pos.x,
            movementBounds.min.x + halfSize.x,
            movementBounds.max.x - halfSize.x);
        pos.y = Mathf.Clamp(pos.y,
            movementBounds.min.y + halfSize.y,
            movementBounds.max.y - halfSize.y);
        return pos;
    }
}