using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneDown : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(col.gameObject);
            GameManager.currentHealth--;
        }
    }
}
