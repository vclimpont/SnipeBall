using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    void Awake()
    {
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

}
