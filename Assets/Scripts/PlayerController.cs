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
    public SpriteRenderer blobBackgroundSprite; 
    public float force;
    public float shootCooldown = 1f;
    public float powerTime = 10f;
    public List<PowerColorAttributes> pcaList;

    public Dictionary<PowerColor, PowerColorAttributes> DictPowerAttributes { get; private set; }

    public Coroutine cOnCooldown { get; private set; }
    private Coroutine cStartPower;

    public Touch currentTouch;
    public Vector3 dragStartPos;
    public bool dragStarted;

    private GameObject currentBlob;
    private PowerColor currentPowerColor;
    private DraggableComponent currentDraggableComponent;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DictPowerAttributes = new Dictionary<PowerColor, PowerColorAttributes>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPowerColorAttributes();

        currentPowerColor = PowerColor.RED;
        currentDraggableComponent = new RedDraggable(this);
        InstantiateNewBlob();
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
                    currentDraggableComponent.OnDragStart(currentBlob);
                    Debug.Log("Began");
                    break;
                case TouchPhase.Moved:
                    currentDraggableComponent.OnDragMoved(currentBlob);
                    break;
                case TouchPhase.Stationary:
                    Debug.Log("Stationary");
                    break;
                case TouchPhase.Ended:
                    currentDraggableComponent.OnDragEnd(currentBlob);
                    Debug.Log("Ended");
                    break;
                case TouchPhase.Canceled:
                    Debug.Log("Canceled");
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            currentDraggableComponent.OnDragStart(currentBlob, true);
        }
        else if(Input.GetMouseButton(0))
        {
            currentDraggableComponent.OnDragMoved(currentBlob, true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            currentDraggableComponent.OnDragEnd(currentBlob, true);
        }
    }

    void SetPowerColorAttributes()
    {
        foreach (PowerColorAttributes pca in pcaList)
        {
            DictPowerAttributes.Add(pca.powerColor, pca);
        }
    }

    DraggableComponent GetCurrentDraggableComponent(PowerColor _powerColor)
    {
        switch (_powerColor)
        {
            case PowerColor.RED:
                return new RedDraggable(this);
            case PowerColor.BLUE:
                return new BlueDraggable(this);
            case PowerColor.GREEN:
                return new GreenDraggable(this);
            case PowerColor.YELLOW:
                return new YellowDraggable(this);
            case PowerColor.ORANGE:
                return new OrangeDraggable(this);
            default:
                return null;
        }
    }

    void SetCurrentBlobAttributes()
    {
        if (currentBlob == null) return;

        blobBackgroundSprite.color = DictPowerAttributes[currentPowerColor].blobBackColor;

        Color healthColor = DictPowerAttributes[currentPowerColor].particlesColor;
        UIManager.Instance.healthSprite.color = new Color(healthColor.r, healthColor.g, healthColor.b, 175f);

        currentBlob.GetComponent<Blob>().SetColorAttributes(DictPowerAttributes[currentPowerColor]);
    }

    public void InstantiateNewBlob()
    {
        currentBlob = Instantiate(blobPrefab, transform);
        SetCurrentBlobAttributes();
    }

    public void StartCooldown()
    {
        cOnCooldown = StartCoroutine(COnCooldown());
    }

    IEnumerator COnCooldown()
    {
        currentBlob = null;

        yield return new WaitForSeconds(shootCooldown);
        InstantiateNewBlob();

        cOnCooldown = null;
        yield return null;
    }

    public void ChangePowerColor(PowerColor _powerColor)
    {
        if(cStartPower != null)
        {
            StopCoroutine(cStartPower);
            currentDraggableComponent.ResetPower();
        }

        cStartPower = StartCoroutine(CStartPower(_powerColor));
    }

    IEnumerator CStartPower(PowerColor _powerColor)
    {
        currentPowerColor = _powerColor;
        currentDraggableComponent = GetCurrentDraggableComponent(_powerColor);
        SetCurrentBlobAttributes();

        currentDraggableComponent.ActivePower();

        yield return new WaitForSeconds(powerTime);

        currentDraggableComponent.ResetPower();

        currentPowerColor = PowerColor.RED;
        currentDraggableComponent = new RedDraggable(this);
        SetCurrentBlobAttributes();

        cStartPower = null;
        yield return null;
    }


}
