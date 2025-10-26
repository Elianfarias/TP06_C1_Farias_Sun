using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerDataSO : ScriptableObject
{
    public KeyCode keyCodeJump = KeyCode.Space;
    public KeyCode keyCodeDown = KeyCode.S;
    public KeyCode keyCodeLeft = KeyCode.A;
    public KeyCode keyCodeRight = KeyCode.D;
    public KeyCode keyCodeDash = KeyCode.LeftShift;
    public KeyCode slowMotionKey = KeyCode.E;
    public Bullet bulletPrefab;
    public int speed;
    public int jumpForce;
    public string playerName;
    public float volumeMusic;
    public float volumeSFX;
    // Bullets
    public int chargerSize = 4;
    public float extraReloadDelay = 2f;
    // dash
    public float dashSpeed = 7f;
    public float inmortalDuration = 0.8f;
    public float dashDuration = 2f;
    public float dashCD = 2f;
    // Jump
    public float timeToFullCharge = 1f;
    public float tapThreshold = 0.3f;
    public float minChargeToRelease = 0.05f;
    public float maxImpulseForce = 8f;
    public bool canChargeWhileDashing = false;
    // slow motion
    public float slowMotionCooldown = 2f;
    public float slowMotionScale = 0.3f;
    public float slowMotionDuration = 2f;
    public int takeDamageSlowMotion = 1;
}