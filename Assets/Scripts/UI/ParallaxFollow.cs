using UnityEngine;

public class ParallaxFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.5f;

    private Transform playerTransform;
    private float startX;

    private void Awake()
    {
        playerTransform = player.transform;
    }

    private void LateUpdate()
    {
        float newX = playerTransform.position.x * parallaxFactor;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}