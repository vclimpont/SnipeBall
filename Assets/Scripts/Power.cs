using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public PlayerController.PowerColor powerColor;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Blob"))
        {
            PlayerController.Instance.ChangePowerColor(powerColor);
            Destroy(gameObject);
        }
    }
}
