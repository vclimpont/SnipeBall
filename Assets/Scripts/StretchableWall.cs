using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchableWall : StretchableComponent
{
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        Rigidbody2D rbCol = col.transform.GetComponent<Rigidbody2D>();
        if (rbCol != null && rbCol.velocity.sqrMagnitude < 3f) return;

        Vector3 upDirection = transform.up.normalized;
        Vector3 impactDirection = ((Vector3)col.GetContact(0).point - transform.position).normalized;

        float absDot = Mathf.Abs(Vector3.Dot(upDirection, impactDirection));
        StretchOnImpact(!(absDot >= 0.25f && absDot <= 0.75f), 0.2f);
    }
}
