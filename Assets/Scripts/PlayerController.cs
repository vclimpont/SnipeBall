using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject blobPrefab;
    public float force;

    private GameObject currentBlob;
    private Touch currentTouch;

    private Vector3 dragStartPos;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            currentTouch = Input.GetTouch(0);

            switch (currentTouch.phase)
            {
                case TouchPhase.Began:
                    OnDragStart();
                    Debug.Log("Began");
                    break;
                case TouchPhase.Moved:
                    OnDragMoved();
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("Stationary");
                    break;
                case TouchPhase.Ended:
                    OnDragEnd();
                    InstantiateNewBall();
                    Debug.Log("Ended");
                    break;
                case TouchPhase.Canceled:
                    Debug.Log("Canceled");
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnDragStart(true);
        }
        else if(Input.GetMouseButton(0))
        {
            OnDragMoved(true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnDragEnd(true);
            InstantiateNewBall();
        }
    }

    void OnDragStart(bool mouse = false)
    {
        dragStartPos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        dragStartPos.z = 0f;
        Debug.Log(dragStartPos);
    }

    void OnDragMoved(bool mouse = false)
    {
        //Move blob according to current touch position
        Vector3 currentDragPosition = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        currentDragPosition.z = 0;

        // Blob rotation
        float angle = Vector3.Angle(Vector3.up, (dragStartPos - currentDragPosition).normalized);
        angle = currentDragPosition.x < dragStartPos.x ? 360f - angle : angle;
        currentBlob.transform.rotation = Quaternion.Euler(0, 0, angle);

        //Blob stretch
    }

    void OnDragEnd(bool mouse = false)
    {
        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        dragReleasePos.z = 0f;

        Vector3 shootDirection = (dragStartPos - dragReleasePos).normalized;
        currentBlob.GetComponent<CircleCollider2D>().enabled = true;
        currentBlob.GetComponent<Rigidbody2D>().gravityScale = -1f;
        currentBlob.GetComponent<Rigidbody2D>().AddForce(shootDirection * force, ForceMode2D.Impulse);
    }

    void InstantiateNewBall()
    {
        currentBlob = Instantiate(blobPrefab, transform);
    }
}
