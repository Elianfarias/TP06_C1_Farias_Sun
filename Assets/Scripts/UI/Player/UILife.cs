using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour
{
    [SerializeField] private HealthSystem target;
    [SerializeField] private Image barLife;

    private void Awake()
    {
        target.onLifeUpdated += HealthSystem_onLifeUpdated;
        target.onDie += HealthSystem_onDie;
    }

    private void OnDestroy()
    {
        target.onLifeUpdated -= HealthSystem_onLifeUpdated;
        target.onDie -= HealthSystem_onDie;
    }

    public void HealthSystem_onLifeUpdated(int current, int max, bool takeDmgMyseft)
    {
        float lerp = current / (float)max;
        barLife.fillAmount = lerp;
    }

    private void HealthSystem_onDie()
    {
        barLife.fillAmount = 0;
    }
}