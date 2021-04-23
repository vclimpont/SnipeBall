using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PowerColorAttributes
{
    public PlayerController.PowerColor powerColor;
    public Material blobMaterial;
    public Color blobBackColor;
    public Color lineRendererStartColor;
    public Color lineRendererEndColor;
    public Color particlesColor;
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public enum PowerColor { RED, BLUE, GREEN, YELLOW, ORANGE }

    public GameObject blobPrefab;
    public float force;
    public float shootCooldown = 1f;
    public float powerTime = 10f;
    public List<PowerColorAttributes> pcaList;

    private Coroutine cOnCooldown;
    private Coroutine cStartPower;

    private LineRenderer line;

    private Dictionary<PowerColor, PowerColorAttributes> dictPowerAttributes;
    private GameObject currentBlob;
    private Touch currentTouch;
    private PowerColor currentPowerColor;

    private Vector3 dragStartPos;

    private bool dragStarted;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            line = GetComponent<LineRenderer>();
            currentPowerColor = PowerColor.RED;
        }
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

    public void ChangePowerColor(PowerColor _pc)
    {
        if(cStartPower != null)
        {
            StopCoroutine(cStartPower);
        }

        cStartPower = StartCoroutine(CStartPower(_pc));
    }

    IEnumerator CStartPower(PowerColor _pc)
    {
        currentPowerColor = _pc;

        yield return new WaitForSeconds(powerTime);

        currentPowerColor = PowerColor.RED;

        cStartPower = null;
        yield return null;
    }
}
