using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    void Start()
    {
        FadeManager.instance.StartFadeIn();
    }

    public void PlayGame()
    {
        FadeManager.instance.StartFadeOut(() => 
        {
            SceneManager.LoadScene(1);
        }); 
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
