using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private EnemySettingsSO data;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem) && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            StartCoroutine(Attack(healthSystem));
    }

    private IEnumerator Attack(HealthSystem healthSystem)
    {
        healthSystem.DoDamage(data.Damage);
        yield return new WaitForSeconds(1f);
    }
}
