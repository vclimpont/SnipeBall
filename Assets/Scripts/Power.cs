using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public PlayerController.PowerColor powerColor;

    public int side;

    private float speed;
    private Vector3 direction;

    void Start()
    {
        speed = Random.Range(0.5f, 3f);
        direction = new Vector3(side == 0 ? 1f : -1f, Random.Range(-2f, 2f), 0).normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Blob"))
        {
            PlayerController.Instance.ChangePowerColor(powerColor);
            Destroy(gameObject);
        }
    }
}
