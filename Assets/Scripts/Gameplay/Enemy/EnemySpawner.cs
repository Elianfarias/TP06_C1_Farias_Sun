using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [SerializeField] private GameObject[] enemiesPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private AudioClip clipSpawn;

    public float lastEnemyActive;
    private float speed = 1.0f;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.PLAYING && (lastEnemyActive < Time.time))
            InvokeEnemy(enemiesPrefab);
    }

    private void InvokeEnemy(GameObject[] enemiesPrefab)
    {

        bool setActive = false;
        Dictionary<int, bool> randIndexesUsed = new();
        float countSpawnersActivated = 0;

        while (!setActive || randIndexesUsed.Count == enemiesPrefab.Length)
        {
            if (randIndexesUsed.Count == enemiesPrefab.Length)
                break;

            var randomIndex = Random.Range(0, enemiesPrefab.Count());

            if (randIndexesUsed.ContainsKey(randomIndex) && randIndexesUsed[randomIndex])
                continue;

            randIndexesUsed[randomIndex] = true;
            var enemySelected = enemiesPrefab[randomIndex];

            if (enemySelected.activeSelf)
                continue;

            enemySelected.transform.position = transform.position;
            enemySelected.SetActive(true);
            countSpawnersActivated++;
            setActive = true;
            AudioController.Instance.PlaySoundEffect(clipSpawn);
        }

        lastEnemyActive = Time.time + Random.Range(1f, spawnInterval + 1);
    }

    public void IncreaseSpeed()
    {
        speed += 0.2f;
    }
}
