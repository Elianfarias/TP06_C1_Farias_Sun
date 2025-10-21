using Assets.Scripts.Gameplay.Player;
using System.Collections;
using UnityEngine;

public class PowerUpInmune : MonoBehaviour, IPowerUp
{
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    [Header("Sound")]
    [SerializeField] private AudioClip powerUpSound;

    public bool isActive = false;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void ApplyPowerUp()
    {
        AudioController.Instance.PlaySoundEffect(powerUpSound);
        isActive = true;
        StartCoroutine(nameof(ApplyInvencible));
    }

    private IEnumerator ApplyInvencible()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
        isActive = false;
    }
}