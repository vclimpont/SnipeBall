using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobWall : MonoBehaviour
{
    public int side;
    public float lifeTime;

    private float targetX;
    private float startX;

    private Coroutine cMoveTo;

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;

        if (side == 0)
        {
            targetX = transform.position.x + 4f;
        }
        else
        {
            targetX = transform.position.x - 4f;
            transform.rotation = Quaternion.Euler(0, 0, 180f);
        }

        StartCoroutine(DestroyAfterLifeTime());
        cMoveTo = StartCoroutine(CMoveTo(targetX, 3f));
    }

    IEnumerator CMoveTo(float x, float moveTime, bool destroy = false)
    {
        Vector3 startPosition = transform.position;
        float dt = 0;

        while (dt <= moveTime)
        {
            transform.position = Vector3.Lerp(startPosition, new Vector3(x, startPosition.y, startPosition.z), dt / moveTime);

            dt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = new Vector3(x, startPosition.y, startPosition.z);

        if (destroy)
            Destroy(gameObject);

        cMoveTo = null;
        yield return null;
    }
     
    IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        if(cMoveTo != null)
        {
            StopCoroutine(cMoveTo);
        }

        cMoveTo = StartCoroutine(CMoveTo(startX, 2f, true));
    }

    void OnDestroy()
    {
        if(WallSpawner.SpawnPositionOnYAxis.Contains(transform.position.y))
        {
            WallSpawner.SpawnPositionOnYAxis.Remove(transform.position.y);
        }
    }
}
