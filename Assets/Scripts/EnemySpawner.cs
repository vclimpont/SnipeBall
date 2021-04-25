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
            Vector3 startPosition = new Vector3(Random.Range(spawnAnchorLeft.position.x, spawnAnchorRight.position.x), spawnAnchorLeft.position.y, spawnAnchorLeft.position.z);
            Vector3 targetPosition = new Vector3(Random.Range(spawnAnchorLeft.position.x, spawnAnchorRight.position.x), spawnAnchorLeft.position.y - Random.Range(2f, 10f), spawnAnchorLeft.position.z);

            GameObject enemyGO = Instantiate(enemyPrefab, startPosition, Quaternion.identity);
            enemyGO.GetComponent<Enemy>().TargetDirection = (targetPosition - startPosition).normalized;

            float scoreScale = Mathf.Clamp(GameManager.score * 0.001f, 0f, 2f);
            yield return new WaitForSeconds(Random.Range(spawnTimer - scoreScale, (spawnTimer + 2f) - (scoreScale * 1.5f)));
        }
    }
}
