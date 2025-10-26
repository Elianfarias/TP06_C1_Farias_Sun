using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private HealthSystem healthSystem;
    [Header("Sound clips")]
    [SerializeField] private AudioClip clipHurt;
    [SerializeField] private AudioClip clipDie;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += HealthSystem_onDie;
        healthSystem.onLifeUpdated += HealthSystem_onLifeUpdated;
        healthSystem.onHealing += HealthSystem_onHealing;
    }

    private void OnDestroy()
    {
        healthSystem.onDie -= HealthSystem_onDie;
        healthSystem.onLifeUpdated -= HealthSystem_onLifeUpdated;
        healthSystem.onHealing += HealthSystem_onHealing;
    }

    private void HealthSystem_onLifeUpdated(int life, int maxLife, bool takeDmgMyselft)
    {
        if(life < maxLife && !takeDmgMyselft)
        {
            CombatEvents.RaiseCameraShake(2f, 0.12f, transform.position);
            AudioController.Instance.PlaySoundEffect(clipHurt, priority: 2);
        }
    }

    private void HealthSystem_onHealing(int life, int maxLife, bool takeDmgMyselft)
    {
    }

    private void HealthSystem_onDie()
    {
        AudioController.Instance.PlaySoundEffect(clipDie, priority: 3);
        GameStateManager.Instance.SetGameState(GameState.GAME_OVER);
    }
}