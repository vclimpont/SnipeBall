using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthSprite;
    public TextMeshProUGUI tmp;
    private Animator tmpAnimator;

    public static UIManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            tmpAnimator = tmp.transform.GetComponent<Animator>();
        }
    }

    void Update()
    {
        healthSprite.fillAmount = (GameManager.currentHealth * 1.0f) / (GameManager.maxHealth * 1.0f);
        tmp.text = "" + GameManager.score;
    }

    public void GainScore()
    {
        tmpAnimator.SetTrigger("TrgStretch");
    }
}
