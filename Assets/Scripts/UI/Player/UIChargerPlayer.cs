using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIChargerPlayer : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private GameObject[] fireballsCharger;

    private void Awake()
    {
        playerAttack.onChargerUpdate += OnChargerUpdate;
        playerAttack.onReload += OnReload;
    }

    private void OnDestroy()
    {
        playerAttack.onChargerUpdate -= OnChargerUpdate;
        playerAttack.onReload -= OnReload;
    }

    private void OnChargerUpdate(int current)
    {
        for (int i = current - 1; i < fireballsCharger.Length; i++)
        {
            fireballsCharger[i].SetActive(false);
        }
    }

    private void OnReload()
    {
        StartCoroutine(nameof(Reload));
    }

    private IEnumerator Reload()
    {
        for (int i = 0; i < fireballsCharger.Length; i++)
        {
            fireballsCharger[i].SetActive(true);
            yield return new WaitForSeconds(playerAttack.data.extraReloadDelay / fireballsCharger.Length);
        }
    }
}