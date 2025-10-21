using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/Enemy")]
public class EnemySettingsSO : ScriptableObject
{
    [SerializeField] private float speedMovement;
    [SerializeField] private int damage;
    [SerializeField] private int timeStun;
    [SerializeField] private int timeMoveSound = 0;

    public float SpeedMovement { get { return speedMovement; } }
    public int Damage { get { return damage; } }
    public int TimeStun { get { return timeStun; } }
    public int TimeMoveSound { get { return timeMoveSound; } }
}
