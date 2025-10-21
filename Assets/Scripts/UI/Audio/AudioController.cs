using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    [SerializeField] private AudioSource soundEffectAudioSource;
    [SerializeField] private AudioSource BackgroundAudioSource;
    
    private int actualPriority = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundEffect(AudioClip audioClip, int priority = 0)
    {
        if (soundEffectAudioSource.isPlaying && actualPriority > priority)
            return;

        actualPriority = priority;
        soundEffectAudioSource.clip = audioClip;
        soundEffectAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        BackgroundAudioSource.Stop();
    }

    public void PlayBackgroundMusic()
    {
        BackgroundAudioSource.Play();
    }
}
