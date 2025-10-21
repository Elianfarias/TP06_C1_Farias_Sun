using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject soul;
    [SerializeField] private EnemySettingsSO data;
    [SerializeField] private AudioClip clipMovement;
    [SerializeField] private AudioClip clipHurt;
    [SerializeField] private AudioClip clipDie;
    [SerializeField] private AudioClip clipAttack;
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private ParticleSystem enemyParticles;

    private HealthSystem healthSystem;
    private EnemyMovement enemyMovement;
    private float nextTimeToReproduce;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.onMove += EnemyMovement_onMove;
        enemyMovement.onAttack += EnemyMovement_onAttack;
        healthSystem.onDie += HealthSystem_onDie;
        healthSystem.onLifeUpdated += HealthSystem_onLifeUpdated;

        nextTimeToReproduce = Time.time;
    }

    private void OnDestroy()
    {
        enemyMovement.onMove -= EnemyMovement_onMove;
        enemyMovement.onAttack -= EnemyMovement_onAttack;
        healthSystem.onDie -= HealthSystem_onDie;
        healthSystem.onLifeUpdated -= HealthSystem_onLifeUpdated;
    }

    private void EnemyMovement_onMove()
    {
        if (nextTimeToReproduce < Time.time)
        {
            nextTimeToReproduce = Time.time + data.TimeMoveSound;
            PlaySoundEffect(clipMovement);
        }
    }

    private void EnemyMovement_onAttack()
    {
        if (nextTimeToReproduce < Time.time)
        {
            nextTimeToReproduce = Time.time + data.TimeStun;
            PlaySoundEffect(clipAttack, priority: true, ignorePlaying: true);
        }
    }

    private void HealthSystem_onDie()
    {
        StartCoroutine(nameof(Die));
    }

    private void HealthSystem_onLifeUpdated(int life, int maxLife)
    {
        StartCoroutine(TakeDamage(life, maxLife));
    }

    private IEnumerator Die()
    {
        PlaySoundEffect(clipDie, priority: true);
        enemyParticles.Play();
        enemyMovement.Die();

        yield return new WaitForSeconds(data.TimeStun);

        soul.transform.position = transform.position + (Vector3.up * 0.2f);
        soul.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator TakeDamage(int life, int maxLife)
    {
        if (life < maxLife)
        {
            PlaySoundEffect(clipHurt, priority: true);
            enemyParticles.Play();
            enemyMovement.StopMovement();

            yield return new WaitForSeconds(data.TimeStun);

            enemyMovement.ResumeMovement();
        }
    }

    private void PlaySoundEffect(AudioClip audioClip, bool priority = false, bool ignorePlaying = false)
    {
        if ((soundEffectAudioSource.isPlaying && !ignorePlaying) && !priority)
            return;

        soundEffectAudioSource.clip = audioClip;
        soundEffectAudioSource.Play();
    }
}
