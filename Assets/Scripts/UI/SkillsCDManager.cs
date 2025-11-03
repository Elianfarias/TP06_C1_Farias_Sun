using Assets.Scripts.Gameplay.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsCDManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private GameObject dashCD;
    [SerializeField] private Image imgMaskDash;

    private void Awake()
    {
        PlayerMovement.OnDashCD += PlayerMovement_onDashCD;
    }

    private void OnDestroy()
    {
        PlayerMovement.OnDashCD -= PlayerMovement_onDashCD;
    }

    private void PlayerMovement_onDashCD(float duration)
    {
        StartCoroutine(DashingCD(duration));
    }

    private IEnumerator DashingCD(float duration)
    {
        float cd = duration;
        dashCD.SetActive(true);
        TextMeshProUGUI txtCDDash = dashCD.GetComponent<TextMeshProUGUI>();

        while (cd > 0)
        {
            cd -= Time.deltaTime;
            imgMaskDash.fillAmount = cd / duration;
            txtCDDash.text = cd.ToString("0.0");

            yield return null;
        }

        dashCD.SetActive(false);
    }
}
