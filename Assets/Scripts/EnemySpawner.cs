using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnAnchorLeft = null;
    [SerializeField] private Transform spawnAnchorRight = null;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private float spawnTimer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CSpawnEnemies());
    }

    private IEnumerator CSpawnEnemies()
    {
        while (true)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(spawnAnchorLeft.position.x, spawnAnchorRight.position.x), spawnAnchorLeft.position.y, spawnAnchorLeft.position.z), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(spawnTimer, spawnTimer + 2f));
        }
    }
}
