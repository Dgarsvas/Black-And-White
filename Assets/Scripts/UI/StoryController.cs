using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public Sprite[] storyImages;
    public Image image;

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
        WaitForSeconds seconds = new WaitForSeconds(8f);
        for (int i = 0; i < storyImages.Length; i++)
        {
            Show(storyImages[i]);
            yield return seconds;
        }
        End();
    }

    private void Show(Sprite sprite)
    {
        image.sprite = sprite;
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
