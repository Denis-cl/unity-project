using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.15f;

    private Bounds levelBounds;
    private Vector3 velocity;          // внутренняя переменная для SmoothDamp
    private Vector3 offset;            // смещение по Z (-10), высчитывается автоматически

    private void Awake()
    {
        offset = new Vector3(0, 0, transform.position.z - target.position.z);
    }

    public void SetBounds(Bounds bounds) => levelBounds = bounds;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position + offset;

        // Плавное следование
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        transform.position = ClampCamera(smoothed);
    }

    private Vector3 ClampCamera(Vector3 pos)
    {
        if (levelBounds.size == Vector3.zero) return pos;

        Camera cam = Camera.main;
        // Полуширина/полувысота видимой области камеры в мировых координатах
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float minX = levelBounds.min.x + halfWidth;
        float maxX = levelBounds.max.x - halfWidth;
        float minY = levelBounds.min.y + halfHeight;
        float maxY = levelBounds.max.y - halfHeight;

        // Если уровень меньше экрана — центрируем, иначе клампим
        pos.x = minX > maxX ? (levelBounds.min.x + levelBounds.max.x) / 2f : Mathf.Clamp(pos.x, minX, maxX);
        pos.y = minY > maxY ? (levelBounds.min.y + levelBounds.max.y) / 2f : Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}
