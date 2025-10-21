using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public event Action<int> onChargerUpdate;
    public event Action onReload;

    [Header("PlayerData")]
    public PlayerDataSO data;
    [SerializeField] private Transform firePoint;
    [Header("Bullets Pooling")]
    [SerializeField] private Bullet[] bulletsPool;
    [Header("Sound clips")]
    [SerializeField] private AudioClip clipFireball;

    private bool canFire = true;
    private int shotsSinceExtraDelay = 0;
    private int nextIndex = 0;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryFire();
    }

    private void TryFire()
    {
        if (!canFire) return;

        Bullet bullet = GetBulletFromPool();
        if (bullet == null) return;

        AudioController.Instance.PlaySoundEffect(clipFireball, priority: 2);
        bullet.gameObject.SetActive(true);
        bullet.transform.SetPositionAndRotation(firePoint.position, Quaternion.identity);
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (targetPos - transform.position).normalized;

        bullet.Shoot(direction);

        // Cooldown
        shotsSinceExtraDelay++;
        float cd = bullet.data.baseCooldown;

        if (shotsSinceExtraDelay >= data.chargerSize)
        {
            onReload.Invoke();
            cd += data.extraReloadDelay;
            shotsSinceExtraDelay = 0;
        }

        StartCoroutine(Cooldown(cd));
    }

    private IEnumerator Cooldown(float seconds)
    {
        canFire = false;
        yield return new WaitForSeconds(seconds);
        canFire = true;
    }

    private Bullet GetBulletFromPool()
    {
        int count = bulletsPool.Length;

        for (int i = 0; i < count; i++)
        {
            var idx = (nextIndex + i) % count;
            if (!bulletsPool[idx].gameObject.activeSelf)
            {
                nextIndex = (idx + 1) % count;
                onChargerUpdate?.Invoke(bulletsPool.Length - nextIndex + 1);
                return bulletsPool[idx];
            }
        }

        return null;
    }
}