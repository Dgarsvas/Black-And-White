using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public Image[] storyImages;

    private bool canSkip;
    private bool isEnding;

    private Coroutine cor;

    public void Start()
    {
        FadeManager.instance.StartFadeIn(() =>
        {
            canSkip = true;
            cor = StartCoroutine(Story());
        });
    }

    private IEnumerator Story()
    {
        WaitForSeconds seconds = new WaitForSeconds(3f);
        for (int i = 0; i < storyImages.Length; i++)
        {
            yield return seconds;
            yield return FadeOut(storyImages[i]);
        }
        End();
    }

    private IEnumerator FadeOut(Image image)
    {
        float timer = 1f;
        while (timer > 0f)
        {
            image.color = new Color(1, 1, 1, timer / 1f);
            timer -= Time.deltaTime;
            yield return null;
        }

        image.color = new Color(1, 1, 1, 0);
    }

    public void Update()
    {
        if (canSkip)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                End();
            }
        }
    }

    private void End()
    {
        if (isEnding)
        {
            return;
        }

        isEnding = true;
        StopCoroutine(cor);
        FadeManager.instance.StartFadeOut(() =>
        {
            SceneManager.LoadScene(2);
        });
    }
}
