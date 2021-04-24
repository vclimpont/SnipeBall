using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float gravityMultiplier = 1f;
    public static float dragMultiplier = 0f;

    public static int currentHealth = 10;
    public static int maxHealth = 10;
    public static int score = 0;

    void Awake()
    {
        currentHealth = maxHealth;
        score = 0;
    }

    public static void GainScore(int scoreAmount)
    {
        score += scoreAmount;
        UIManager.Instance.GainScore();
    }

    public static void RecoverHealth()
    {
        if (currentHealth >= maxHealth) return;

        currentHealth++;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Debug.Log("GAME OVER");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
