using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTracker : MonoBehaviour
{
    public Text text;

    void Start()
    {
        text.text = "" + SaveManager.Instance.state.highscore;
    }
}
