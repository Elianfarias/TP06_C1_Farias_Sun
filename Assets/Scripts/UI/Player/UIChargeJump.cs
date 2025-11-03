using Assets.Scripts.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

public class UIChargerJump : MonoBehaviour
{
    [SerializeField] private PlayerMovement target;
    [SerializeField] private Image barCharger;

    private void Awake()
    {
        target.OnChargerJump += OnChargerJump;
        target.OnJump += OnJump;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        target.OnChargerJump -= OnChargerJump;
        target.OnJump -= OnJump;
    }

    public void OnChargerJump(float currentCharge, float maxCharge)
    {
        gameObject.SetActive(true);
        float lerp = currentCharge / maxCharge;
        barCharger.fillAmount = lerp;
    }

    public void OnJump(bool jump)
    {
        gameObject.SetActive(false);
    }
}