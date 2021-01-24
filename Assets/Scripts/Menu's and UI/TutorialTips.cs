using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTips : MonoBehaviour
{
    private Text _text;
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    public void DisplayText(string text)
    {
        StopAllCoroutines();
        StartCoroutine(DoDisplayText(text));
    }

    IEnumerator DoDisplayText(string text)
    {
        _text.text = text;
        _text.color = new Color(1, 1, 1, 1);

        float solidTimer = 2.0f;

        while (solidTimer >= 0.0f)
        {
            solidTimer -= Time.deltaTime;
            yield return null;
        }
        const float fadeTimerMax = 4.0f;
        float fadeTimer = fadeTimerMax;

        while (fadeTimer >= 0.0f)
        {
            fadeTimer-= Time.deltaTime;

            _text.color = new Color(1, 1, 1, fadeTimer/fadeTimerMax);
            yield return null;
        }
        _text.color = new Color(1, 1, 1, 0.0f);

        yield return null;
    }
}
