using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerDataSO : ScriptableObject
{
    public KeyCode keyCodeJump = KeyCode.Space;
    public KeyCode keyCodeDown = KeyCode.S;
    public KeyCode keyCodeLeft = KeyCode.A;
    public KeyCode keyCodeRight = KeyCode.D;
    public KeyCode keyCodeDash = KeyCode.LeftShift;
    public Bullet bulletPrefab;
    public int speed;
    public int jumpForce;
    public string playerName;
    public float volumeMusic;
    public float volumeSFX;
    public int chargerSize = 4;
    public float dashSpeed = 7f;
    public float inmortalDuration = 0.8f;
    public float dashDuration = 2f;
    public float extraReloadDelay = 2f;
}