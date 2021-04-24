using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : ScorableComponent
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer = null;
    [SerializeField] private TrailRenderer trailRenderer = null;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Die()
    {
        rb.velocity = Vector2.zero;
        Destroy(gameObject);
    }

    public void Shoot(Vector3 direction, float force)
    {
        circleCollider.enabled = true;
        rb.gravityScale = -1f;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void Stretch(float distValue)
    {
        float stretchX = Mathf.Clamp01(distValue * 0.5f) * 0.25f;
        float stretchY = Mathf.Clamp01(distValue * 0.5f) * 0.75f;
        blobMesh.transform.localScale = Vector3.one + new Vector3(-stretchX, stretchY, 0f);
    }

    public void SetColorAttributes(PowerColorAttributes pca)
    {
        meshRenderer.material = pca.blobMaterial;
        trailRenderer.startColor = pca.lineRendererStartColor;
        trailRenderer.endColor = pca.lineRendererEndColor;
    }
}
