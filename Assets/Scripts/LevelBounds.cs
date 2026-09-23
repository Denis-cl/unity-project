using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [SerializeField] private BoxCollider2D arena;       // коллайдер-арена
    [SerializeField] private PlayerController player;
    [SerializeField] private CameraFollow cameraFollow;

    private void Start()
    {
        Bounds b = arena.bounds;
        if (player != null) player.SetBounds(b);
        if (cameraFollow != null) cameraFollow.SetBounds(b);
    }
}