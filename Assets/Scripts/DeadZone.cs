using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") || col.gameObject.layer == LayerMask.NameToLayer("Blob"))
        {
            Destroy(col.gameObject);
        }
    }
}
