using Assets.Scripts.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

public class UIChargerJump : MonoBehaviour
{
    [SerializeField] private PlayerMovement target;
    [SerializeField] private Image barCharger;

    private void Awake()
    {
        target.onChargerJump += onChargerJump;
        target.onJump += onJump;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        target.onChargerJump -= onChargerJump;
        target.onJump -= onJump;
    }

    public void onChargerJump(float currentCharge, float maxCharge)
    {
        gameObject.SetActive(true);
        float lerp = currentCharge / maxCharge;
        barCharger.fillAmount = lerp;
    }

    public void onJump(bool jump)
    {
        gameObject.SetActive(false);
    }
}