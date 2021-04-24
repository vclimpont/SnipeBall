using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorableComponent : MonoBehaviour
{
    public GameObject scorePrefab;
    public float timerReset = 2f;

    private Coroutine cStartTimer;

    private int currentScoreReward = 0;

    void OnCollisionEnter2D(Collision2D col)
    {
        ScorableComponent sc = col.transform.GetComponent<ScorableComponent>();
        if(sc != null)
        {
            currentScoreReward += 10;
            TextMesh mt = Instantiate(scorePrefab, col.GetContact(0).point + new Vector2(Random.Range(-1f, 1f), 0), Quaternion.identity).GetComponent<ScoreText>().textMesh;
            mt.text = "" + currentScoreReward;
            mt.characterSize = Mathf.Clamp(0.35f + (0.005f * currentScoreReward), 0.35f, 0.75f);

            if(currentScoreReward % 100 == 0)
            {
                GameManager.RecoverHealth();
            }

            GameManager.score += currentScoreReward;

            if (cStartTimer != null)
            {
                StopCoroutine(cStartTimer);
            }
            cStartTimer = StartCoroutine(CStartTimer());
        }
    }

    IEnumerator CStartTimer()
    {
        yield return new WaitForSeconds(timerReset);

        currentScoreReward = 0;
        cStartTimer = null;

        yield return null;
    }
}
