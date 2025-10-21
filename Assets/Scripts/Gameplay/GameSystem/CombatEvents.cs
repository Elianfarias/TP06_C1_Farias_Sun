using System;
using UnityEngine;

public static class CombatEvents
{
    public static event Action<float, float, Vector3> OnCameraShake;

    public static void RaiseCameraShake(float intensity, float duration, Vector3 hitPos)
        => OnCameraShake?.Invoke(intensity, duration, hitPos);
}
