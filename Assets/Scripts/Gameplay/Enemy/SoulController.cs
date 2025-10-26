using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    [SerializeField] private AudioClip clipTakeSoul;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && 
            collision.gameObject.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.Heal(20);
            AudioController.Instance.PlaySoundEffect(clipTakeSoul, priority: 3);
            ScoreManager.Instance.AddSoul();
            gameObject.SetActive(false);
        }
    }
}
