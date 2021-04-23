using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blobWallPrefab = null;
    [SerializeField] private Transform anchorLeft = null;
    [SerializeField] private Transform anchorRight = null;
    [SerializeField] private Transform anchorBotLeft = null;
    [SerializeField] private Transform anchorBotRight = null;

    public float timeBeforeSpawningFirstWall = 0f;
    public float spawnMinRate = 5f;
    public float spawnMaxRate = 20f;
    public float wallLifeTimeMin = 7f;
    public float wallLifeTimeMax = 12f;

    public static List<float> SpawnPositionOnYAxis { get; set; }

    void Awake()
    {
        SpawnPositionOnYAxis = new List<float>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CSpawnWalls());
    }

    private IEnumerator CSpawnWalls()
    {
        yield return new WaitForSeconds(timeBeforeSpawningFirstWall);

        while (true)
        {
            int s = Random.Range(0, 2); // rand between 0 1
            float x = s == 0 ? Random.Range(anchorLeft.position.x - 5f, anchorLeft.position.x - 1f) : Random.Range(anchorRight.position.x + 1f, anchorRight.position.x + 5f);

            float y;
            do
            {
                y = Random.Range(anchorBotLeft.position.y, anchorLeft.position.y);

            } while (SpawnPositionOnYAxis.FindAll(yy => Mathf.Abs(y - yy) < 1f).Count > 0); // avoid overlapping walls

            BlobWall bw = Instantiate(blobWallPrefab, new Vector3(x, y, 0), Quaternion.identity).GetComponent<BlobWall>();
            bw.side = s;
            bw.lifeTime = Random.Range(wallLifeTimeMin, wallLifeTimeMax);

            SpawnPositionOnYAxis.Add(bw.transform.position.y);

            yield return new WaitForSeconds(Random.Range(spawnMinRate, spawnMaxRate));
        }
    }
}
