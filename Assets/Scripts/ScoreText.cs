using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{ 
    public TextMesh textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMesh>();
    }

    void Start()
    {
        StartCoroutine(CFade());
        Destroy(gameObject, 1.25f);
    }

    IEnumerator CFade()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, 0.3f, 0);

        float dt = 0f;
        float fadeSpeed = 1f;

        while (dt <= fadeSpeed)
        {
            float a = Mathf.Lerp(0f, 1f, 1f - dt / fadeSpeed);
            Color c = textMesh.color;
            textMesh.color = new Color(c.r, c.g, c.b, a);

            transform.position = Vector3.Lerp(startPosition, targetPosition, dt / fadeSpeed);

            dt += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
