using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] GameObject dieImpact = null;
    void OnTriggerEnter2D(Collider2D col)
    {
        bool enemy = col.gameObject.layer == LayerMask.NameToLayer("Enemy");
        bool blob = col.gameObject.layer == LayerMask.NameToLayer("Blob");
        if (enemy || blob)
        {
            if(enemy)
            {
                Destroy(Instantiate(dieImpact, col.transform.position - new Vector3(0, 0.5f, 0), Quaternion.Euler(90f, 0, 0)), 2f);
            }
            Destroy(col.gameObject);
        }
    }
}
