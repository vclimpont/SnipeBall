using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject blobPrefab;
    public float force;
    public float shootCooldown = 1f;

    private LineRenderer line;

    private GameObject currentBlob;
    private Touch currentTouch;

    private Vector3 dragStartPos;

    private Coroutine cOnCooldown;

    private bool dragStarted;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

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
        }
    }

    void OnDragStart(bool mouse = false)
    {
        if (cOnCooldown != null) return;
        dragStarted = true;

        dragStartPos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        dragStartPos.z = 0f;

        line.positionCount = 2;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
    }

    void OnDragMoved(bool mouse = false)
    {
        if (!dragStarted) return;

        //Move blob according to current touch position
        Vector3 currentDragPosition = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        currentDragPosition.z = 0;

        Vector3 localDragPosition = currentDragPosition + (transform.position - dragStartPos);
        line.SetPosition(1, transform.position + Vector3.ClampMagnitude((transform.position - localDragPosition), 10f));

        // Blob rotation
        float angle = Vector3.Angle(Vector3.up, (dragStartPos - currentDragPosition).normalized);
        angle = currentDragPosition.x < dragStartPos.x ? 360f - angle : angle;
        currentBlob.transform.rotation = Quaternion.Euler(0, 0, angle);

        //Blob stretch
        float dist = Vector3.Distance(dragStartPos, currentDragPosition);
        currentBlob.GetComponent<Blob>().Stretch(dist);
    }

    void OnDragEnd(bool mouse = false)
    {
        if (!dragStarted) return;

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(mouse ? Input.mousePosition : (Vector3)currentTouch.position);
        dragReleasePos.z = 0f;

        line.positionCount = 0;

        if (Vector3.Distance(dragStartPos, dragReleasePos) < 0.5f)
        {
            Destroy(currentBlob);
            InstantiateNewBall();
            dragStarted = false;
            return;
        }

        Vector3 shootDirection = (dragStartPos - dragReleasePos).normalized;
        currentBlob.GetComponent<Blob>().Shoot(shootDirection, force);

        cOnCooldown = StartCoroutine(COnCooldown());
        dragStarted = false;
    }

    void InstantiateNewBall()
    {
        currentBlob = Instantiate(blobPrefab, transform);
    }

    IEnumerator COnCooldown()
    {
        yield return new WaitForSeconds(shootCooldown);
        InstantiateNewBall();

        cOnCooldown = null;
        yield return null;
    }
}
