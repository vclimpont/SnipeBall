using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchableComponent : MonoBehaviour
{
    public GameObject blobMesh = null;
    public Vector3 baseScale = Vector3.one;

    private Coroutine cStretchOnImpact;

    protected virtual void Awake()
    {
    }

    protected void StretchOnImpact(bool vertical, float stretchValue)
    {
        if(cStretchOnImpact != null)
        {
            StopCoroutine(cStretchOnImpact);
        }

        float x = vertical ? baseScale.x - stretchValue : baseScale.x + stretchValue;
        float y = vertical ? baseScale.x + stretchValue : baseScale.x - stretchValue;
        cStretchOnImpact = StartCoroutine(CStretch(x, y));
    }

    IEnumerator CStretch(float x, float y)
    {
        float dt = 0f;
        float stretchSpeed = 0.1f;

        while (dt <= stretchSpeed)
        {
            blobMesh.transform.localScale = Vector3.Lerp(baseScale, new Vector3(x, y, 1), dt / stretchSpeed);
            dt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        blobMesh.transform.localScale = new Vector3(x, y, 1);

        dt = 0f;
        stretchSpeed = 0.15f;

        while (dt <= stretchSpeed)
        {
            blobMesh.transform.localScale = Vector3.Lerp(new Vector3(x, y, 1), baseScale, dt / stretchSpeed);
            dt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        blobMesh.transform.localScale = baseScale;

        cStretchOnImpact = null;
        yield return null;
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D rbCol = col.transform.GetComponent<Rigidbody2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if ((rbCol != null && rbCol.velocity.sqrMagnitude < 1f) && (rb.velocity.sqrMagnitude < 2f)) return;

        Vector3 upDirection = transform.up.normalized;
        Vector3 impactDirection = ((Vector3)col.GetContact(0).point - transform.position).normalized;

        float absDot = Mathf.Abs(Vector3.Dot(upDirection, impactDirection));
        StretchOnImpact(absDot >= 0.3f && absDot <= 0.6f, 0.33f);
    }
}
