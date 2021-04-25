using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    public void LaunchGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
