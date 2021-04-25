using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorableComponent : StretchableComponent
{
    public GameObject impactPrefab;
    public Color impactParticlesColor;
    public GameObject scorePrefab;
    public float timerReset = 2f;

    private Coroutine cStartTimer;

    private int currentScoreReward = 0;

    IEnumerator CStartTimer()
    {
        yield return new WaitForSeconds(timerReset);

        currentScoreReward = 0;
        cStartTimer = null;

        yield return null;
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        ScorableComponent sc = col.transform.GetComponent<ScorableComponent>();
        if(sc != null)
        {
            currentScoreReward += 10;
            ScoreText st = Instantiate(scorePrefab, col.GetContact(0).point + new Vector2(Random.Range(-1f, 1f), 0), Quaternion.identity).GetComponent<ScoreText>();
            TextMeshPro mt = st.textMesh;
            mt.text = "" + currentScoreReward;
            mt.fontSize = Mathf.Clamp(10f + (0.05f * currentScoreReward), 10f, 15f);

            if(currentScoreReward % 100 == 0)
            {
                mt.fontSize = 17f;
                mt.fontStyle = FontStyles.Bold;
                mt.color = PlayerController.Instance.GetCurrentPCA().particlesColor;
                st.healPlayer = true;
            }

            GameObject impactGO = Instantiate(impactPrefab, col.GetContact(0).point, Quaternion.identity);
            var main = impactGO.GetComponent<ParticleSystem>().main;
            main.startColor = impactParticlesColor;
            Destroy(impactGO, 1.5f);

            GameManager.GainScore(currentScoreReward);

            if (cStartTimer != null)
            {
                StopCoroutine(cStartTimer);
            }
            cStartTimer = StartCoroutine(CStartTimer());
        }
    }
}
