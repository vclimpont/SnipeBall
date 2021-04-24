using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthSprite;
    public TextMeshProUGUI tmp;

    public static UIManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        healthSprite.fillAmount = (GameManager.currentHealth * 1.0f) / (GameManager.maxHealth * 1.0f);
        tmp.text = "" + GameManager.score;
    }
}
