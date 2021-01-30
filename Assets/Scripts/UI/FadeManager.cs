using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;
    public Image fade;
    public AnimationCurve curve;
    public float fadeTime;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartFadeIn(Action onComplete = null)
    {
        StartCoroutine(FadeIn(onComplete));
    }

    public void StartFadeOut(Action onComplete = null)
    {
        StartCoroutine(FadeOut(onComplete));
    }

    private IEnumerator FadeIn(Action onComplete)
    {
        float timer = fadeTime;
        while (timer > 0f)
        {
            fade.color = new Color(0, 0, 0, curve.Evaluate(timer / fadeTime));
            timer -= Time.deltaTime;
            yield return null;
        }

        fade.color = new Color(0, 0, 0, 0);
        onComplete?.Invoke();
    }

    private IEnumerator FadeOut(Action onComplete)
    {
        float timer = 0f;
        while (timer < fadeTime)
        {
            fade.color = new Color(0, 0, 0, curve.Evaluate(timer / fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }

        fade.color = new Color(0, 0, 0, 1);
        onComplete?.Invoke();
    }
}