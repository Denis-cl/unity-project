using UnityEngine;

/// <summary>
/// Следование камеры за игроком с ограничением границами уровня.
///
/// Сглаживание (SmoothDamp/Lerp) сознательно НЕ используется:
/// игрок двигается в FixedUpdate (50 Гц), а камера — каждый кадр,
/// и смешение частот давало периодические подёргивания (биения).
/// Для жанра «аркада на выживание» жёсткое следование правильнее —
/// оно даёт ощущение полного контроля (см. GDD, раздел «feel»).
///
/// Ссылка на камеру кэшируется в Awake, чтобы не вызывать Camera.main
/// каждый кадр (поиск по сцене создавал бы мусор и микро-зависания от GC).
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;   // Цель слежения (игрок)

    private Camera cam;                          // Кэш компонента Camera
    private Bounds levelBounds;                  // Границы уровня (из LevelBounds)
    private Vector3 offset;                      // Смещение камеры по Z

    private void Awake()
    {
        cam = GetComponent<Camera>();            // Запоминаем камеру один раз при старте
        offset = new Vector3(0, 0, transform.position.z - target.position.z);
    }

    /// <summary>Границы уровня задаются извне — скриптом LevelBounds при старте сцены.</summary>
    public void SetBounds(Bounds bounds) => levelBounds = bounds;

    private void LateUpdate()
    {
        if (target == null) return;

        // Жёсткое следование: позиция камеры = позиция игрока + смещение по Z.
        // Плавность обеспечивается самим движением игрока (интерполяция Rigidbody2D).
        Vector3 targetPos = target.position + offset;

        // Ограничение границами уровня: камера не показывает пустоту за ареной
        transform.position = ClampCamera(targetPos);
    }

    /// <summary>
    /// Прижимает позицию камеры к границам уровня с учётом размера видимой области.
    /// Если уровень меньше экрана — камера центрируется на нём.
    /// </summary>
    private Vector3 ClampCamera(Vector3 pos)
    {
        if (levelBounds.size == Vector3.zero) return pos;

        // Используем закэшированную камеру — без лишних аллокаций каждый кадр
        float halfHeight = cam.orthographicSize;            // Полувысота видимой области
        float halfWidth = halfHeight * cam.aspect;          // Полуширина (с учётом пропорций экрана)

        float minX = levelBounds.min.x + halfWidth;
        float maxX = levelBounds.max.x - halfWidth;
        float minY = levelBounds.min.y + halfHeight;
        float maxY = levelBounds.max.y - halfHeight;

        // Если уровень уже экрана — центрируемся, иначе клампим внутрь границ
        pos.x = minX > maxX ? (levelBounds.min.x + levelBounds.max.x) / 2f : Mathf.Clamp(pos.x, minX, maxX);
        pos.y = minY > maxY ? (levelBounds.min.y + levelBounds.max.y) / 2f : Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}