using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScorableComponent
{
    public float baseSpeed;
    public float maxSpeed;

    public Vector3 TargetDirection { get; set; }

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.AddForce((Vector2)TargetDirection * (baseSpeed * Random.Range(1f, 1f)), ForceMode2D.Impulse);
    }

    void Update()
    {
        rb.gravityScale = 0.5f * GameManager.gravityMultiplier;
        rb.drag = GameManager.dragMultiplier;
    }
}
