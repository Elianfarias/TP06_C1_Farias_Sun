using UnityEngine;

[CreateAssetMenu(fileName = "FireballStats", menuName = "ScriptableObjects/Bullets")]
public class FireballSO : ScriptableObject
{
    public int damage = 20;
    public int speed = 30;
    public float baseCooldown = 0.2f;
}