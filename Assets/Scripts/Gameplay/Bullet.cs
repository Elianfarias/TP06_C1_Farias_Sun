using UnityEngine;

public class Bullet : MonoBehaviour
{
    public FireballSO data;

    private Rigidbody2D rb;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 direction = rb.velocity.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthSystem healthSystem))
        {
            CombatEvents.RaiseCameraShake(2f, 0.12f, transform.position);
            healthSystem.DoDamage(data.damage);
        }

        gameObject.SetActive(false);
    }

    public void Shoot(Vector3 direction)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction.normalized * data.speed, ForceMode2D.Impulse);
    }
}