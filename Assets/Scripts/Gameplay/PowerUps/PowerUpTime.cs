using UnityEngine;

public class PowerUpTime : MonoBehaviour, IPowerUp
{
    [SerializeField] private float speed;
    [Header("Sound")]
    [SerializeField] private AudioClip powerUpSound;

    public bool isActive = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * Time.fixedDeltaTime * Vector2.left;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void ApplyPowerUp()
    {
        AudioController.Instance.PlaySoundEffect(powerUpSound);
        isActive = true;
        gameObject.SetActive(false);
        isActive = false;
    }
}
