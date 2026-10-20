using UnityEngine;

/// <summary>
/// Конечный автомат врага (FSM на enum + switch).
/// Состояния: Idle → Chase → Attack → Dead.
/// Вынесен в отдельный класс: поведение инкапсулировано от остальных
/// компонентов (HP в EnemyHealth, параметры в EnemyStats).
/// Читаемость для игрока: цвет спрайта + логи в консоль.
/// Защита: таблица допустимых переходов, Dead — терминальное.
/// </summary>
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState { Idle, Chase, Attack, Dead }

    [Header("Ссылки")]
    [SerializeField] private EnemyStats stats;          // Параметры из SO (data-driven)
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;   // Можно оставить пустым

    /// <summary>Точка вылета: FirePoint, а если не назначен — центр врага.</summary>
    private Vector2 FirePos => firePoint != null
        ? (Vector2)firePoint.position
        : (Vector2)transform.position;

    [Header("Детекция")]
    [SerializeField, Tooltip("Радиус, в котором враг замечает игрока (Idle → Chase)")]
    private float aggroRadius = 8f;
    [SerializeField, Tooltip("Радиус потери игрока (Chase/Attack → Idle)")]
    private float loseRadius = 12f;
    [SerializeField, Tooltip("Длительность покоя (случайно от X до Y сек)")]
    private Vector2 idleDuration = new Vector2(2f, 5f);

    private EnemyState currentState;
    private float stateTimer;        // Время в текущем состоянии
    private float idleUntil;         // Когда кончится покой
    private float nextShotTime;      // Кулдаун стрельбы
    private SpriteRenderer sprite;
    private Transform player;

    // ---------- Жизненный цикл ----------

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        // Смерть приходит из EnemyHealth — подписываемся на событие
        var health = GetComponent<EnemyHealth>();
        if (health != null) health.OnDied += HandleDied;
    }

    private void OnDestroy()
    {
        var health = GetComponent<EnemyHealth>();
        if (health != null) health.OnDied -= HandleDied;
    }

    private void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        OnStateEnter(EnemyState.Idle);   // Начальное состояние
    }

    // ---------- Главный цикл: switch по состояниям ----------

    private void Update()
    {
        stateTimer += Time.deltaTime;
        if (player == null || stats == null) return;

        // Мёртвый враг ничего не делает
        if (currentState == EnemyState.Dead) return;

        float dist = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:
                // Стоим, «дремлем»; заметили игрока → преследование
                if (dist <= aggroRadius)
                    ChangeState(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                if (dist > loseRadius) { ChangeState(EnemyState.Idle); break; }
                if (dist <= stats.StopDistance) { ChangeState(EnemyState.Attack); break; }
                transform.position = Vector2.MoveTowards(
                    transform.position, player.position,
                    stats.MoveSpeed * Time.deltaTime);
                break;

            case EnemyState.Attack:
                // Гистерезис: выход из атаки чуть дальше, чем вход —
                // чтобы состояние не дребезжало на границе
                if (dist > loseRadius) { ChangeState(EnemyState.Idle); break; }
                if (dist > stats.StopDistance + 0.5f) { ChangeState(EnemyState.Chase); break; }

                if (Time.time >= nextShotTime)
                {
                    nextShotTime = Time.time + stats.FireInterval;
                    Shoot();
                }
                break;
        }
    }

    // ---------- Переходы ----------

    /// <summary>Запрос перехода: сначала проверка допустимости (защита FSM).</summary>
    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        if (!CanTransition(currentState, newState))
        {
            Debug.LogWarning($"[EnemyFSM] ЗАПРЕЩЁННЫЙ переход: {currentState} → {newState}!");
            return;
        }

        OnStateExit(currentState);
        currentState = newState;
        stateTimer = 0f;
        OnStateEnter(newState);
    }

    /// <summary>Таблица допустимых переходов. Dead — терминальное (из него нельзя).</summary>
    private bool CanTransition(EnemyState from, EnemyState to)
    {
        switch (from)
        {
            case EnemyState.Idle:
                return to == EnemyState.Chase || to == EnemyState.Dead;
            case EnemyState.Chase:
                return to == EnemyState.Attack || to == EnemyState.Idle || to == EnemyState.Dead;
            case EnemyState.Attack:
                return to == EnemyState.Chase || to == EnemyState.Idle || to == EnemyState.Dead;
            case EnemyState.Dead:
                return false;   // терминальное состояние
            default:
                return false;
        }
    }

    private void HandleDied() => ChangeState(EnemyState.Dead);

    // ---------- Enter / Exit ----------

    private void OnStateEnter(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                idleUntil = Time.time + Random.Range(idleDuration.x, idleDuration.y);
                SetColor(Color.green);
                break;
            case EnemyState.Chase:
                SetColor(Color.yellow);
                break;
            case EnemyState.Attack:
                SetColor(Color.red);
                nextShotTime = Time.time;   // первый выстрел — сразу при входе
                break;
            case EnemyState.Dead:
                SetColor(Color.gray);
                break;
        }
        Debug.Log($"[EnemyFSM] → {state} (вход, время в прошлом состоянии: {stateTimer:F1}с не учтено)");
    }

    private void OnStateExit(EnemyState state)
    {
        Debug.Log($"[EnemyFSM] {state} → (выход, прожито {stateTimer:F1}с)");
    }

    // ---------- Прочее ----------

    private void Shoot()
    {
        Vector2 dir = ((Vector2)player.position - FirePos).normalized;
        var bullet = Instantiate(bulletPrefab, FirePos, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().Init(dir, stats.BulletSpeed, stats.BulletDamage);
    }

    private void SetColor(Color c)
    {
        if (sprite != null) sprite.color = c;
    }
}
