using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [Header("Opciones")]
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private Transform listenerAnchor;
    [SerializeField] private bool useDistanceFalloff = true;

    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;
    private float startIntensity;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (listenerAnchor == null) listenerAnchor = transform;
    }

    private void OnEnable()
    {
        CombatEvents.OnCameraShake += HandleShake;
    }

    private void OnDisable()
    {
        CombatEvents.OnCameraShake -= HandleShake;
    }

    private void HandleShake(float intensity, float duration, Vector3 hitPos)
    {
        float finalIntensity = intensity;

        if (useDistanceFalloff && listenerAnchor != null)
        {
            float d = Vector3.Distance(listenerAnchor.position, hitPos);
            float t = Mathf.Clamp01(1f - (d / maxDistance));
            finalIntensity *= t;
            if (finalIntensity <= 0.01f) return;
        }

        perlin.m_AmplitudeGain = Mathf.Max(perlin.m_AmplitudeGain, finalIntensity);
        startIntensity = perlin.m_AmplitudeGain;
        shakeTimer = Mathf.Max(shakeTimer, duration);
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            float t = (shakeTimer <= 0f) ? 0f : (shakeTimer / Mathf.Max(shakeTimer + Time.deltaTime, 0.0001f));
            perlin.m_AmplitudeGain = Mathf.Lerp(0f, startIntensity, t);

            if (shakeTimer <= 0f)
                perlin.m_AmplitudeGain = 0f;
        }
    }
}
