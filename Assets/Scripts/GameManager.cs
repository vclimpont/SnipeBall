using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static float gravityMultiplier = 1f;
    public static float dragMultiplier = 0f;

    public static int currentHealth = 5;
    public static int maxHealth = 5;
    public static int score = 0;

    private bool gameOver;

    void Awake()
    {
        currentHealth = maxHealth;
        gameOver = false;
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
        if (gameOver) return;

        if(currentHealth <= 0)
        {
            gameOver = true;
            Time.timeScale = 0f;

            bool newRecord = false;

            if(SaveManager.Instance.state.highscore < score)
            {
                SaveManager.Instance.state.highscore = score;
                SaveManager.Instance.Save();
                newRecord = true;
            }

            UIManager.Instance.ShowGameOver(newRecord);
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainGame");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Pregame");
    }
}
