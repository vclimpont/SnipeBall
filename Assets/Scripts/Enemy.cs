using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float baseSpeed;
    public float maxSpeed;

    private Rigidbody2D rb;

    private bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = Vector2.down * (baseSpeed * Random.Range(1f, 3f));
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Blob"))
        {
            Die();
        }
    }
}
