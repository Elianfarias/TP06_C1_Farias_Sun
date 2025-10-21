using System.Collections;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("PowerUp/Blocks")]
    [SerializeField] private GameObject[] powerUps;
    [Header("Limits")]
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    private void Start()
    {
        StartCoroutine(nameof(SpawnPowerUp));
    }


    private IEnumerator SpawnPowerUp()
    {
        float randomSeconds = Random.Range(3f, 7f);
        yield return new WaitForSeconds(randomSeconds);

        int randomIndex = Random.Range(0, powerUps.Length);
        float randomPositionX = Random.Range(minPosition.x, maxPosition.x);
        float randomPositionY = Random.Range(minPosition.y, maxPosition.y);
        GameObject powerUpSelected = powerUps[randomIndex];

        powerUpSelected.SetActive(true);
        powerUpSelected.transform.position = new Vector2(randomPositionX, randomPositionY);

        yield return new WaitForSeconds(6f);
        
        if (!powerUpSelected.GetComponent<IPowerUp>().IsActive())
            powerUpSelected.SetActive(false);
        
        StartCoroutine(nameof(SpawnPowerUp));
    }
}
