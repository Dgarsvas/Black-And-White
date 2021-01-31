using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;

    public GameObject winScreen, looseScreen, objectivePanel;

    private bool screenShown;

    internal void ChangeObjective(string text)
    {
        objectivePanel.SetActive(true);
        objectiveText.text = text;
    }

    internal void ShowWinScreen()
    {
        screenShown = true;
        objectivePanel.SetActive(false);
        winScreen.SetActive(true);
    }

    internal void ShowLooseScreen()
    {
        screenShown = true;
        objectivePanel.SetActive(false);
        looseScreen.SetActive(true);
    }

    public void Update()
    {
        if (screenShown)
        {
            if (Input.anyKeyDown)
            {
                GoBackToMenu();
            }
        }
    }

    public void GoBackToMenu()
    {
        FadeManager.instance.StartFadeOut(() => 
        {
            SceneManager.LoadScene(0);
        });
    }
}
