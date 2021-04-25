using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ScorableComponent
{
    public float baseSpeed;
    public float maxSpeed;

    public Vector3 TargetDirection { get; set; }

    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        float scoreScale = Mathf.Clamp(GameManager.score * 0.0005f, 0f, 2f);
        float f = GameManager.score >= 100000 ? 1.5f : 1f; 
        rb.AddForce((Vector2)TargetDirection * f * (Random.Range(baseSpeed + scoreScale, maxSpeed + scoreScale)), ForceMode2D.Impulse);
    }

    void Update()
    {
        rb.gravityScale = 0.05f * GameManager.gravityMultiplier;
        rb.drag = GameManager.dragMultiplier;
    }
}
