using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerPrefabs = null;
    [SerializeField] private Transform anchorLeft = null;
    [SerializeField] private Transform anchorRight = null;
    [SerializeField] private Transform anchorBotLeft = null;
    [SerializeField] private Transform anchorBotRight = null;

    public float timeBeforeSpawningFirstPower = 0f;
    public float spawnMinRate = 10f;
    public float spawnMaxRate = 20f;
    public float powerLifeTime = 20f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CSpawnPower());
    }

    private IEnumerator CSpawnPower()
    {
        yield return new WaitForSeconds(timeBeforeSpawningFirstPower);

        while (true)
        {
            int s = Random.Range(0, 2); // rand between 0 1
            float x = s == 0 ? anchorLeft.position.x - 1f : anchorRight.position.x + 1f;

            float y;
            y = Random.Range(anchorBotLeft.position.y + 2f, anchorLeft.position.y - 2f);

            Power power = Instantiate(powerPrefabs[Random.Range(0, powerPrefabs.Length)], new Vector3(x, y, 0), Quaternion.identity).GetComponent<Power>();
            power.side = s;

            Destroy(power.gameObject, powerLifeTime);

            yield return new WaitForSeconds(Random.Range(spawnMinRate, spawnMaxRate));
        }
    }
}
