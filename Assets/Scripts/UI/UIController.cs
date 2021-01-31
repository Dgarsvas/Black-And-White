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

    internal void ChangeObjective(string text)
    {
        objectivePanel.SetActive(true);
        objectiveText.text = text;
    }

    internal void ShowWinScreen()
    {
        objectivePanel.SetActive(false);
        winScreen.SetActive(true);
    }

    internal void ShowLooseScreen()
    {
        objectivePanel.SetActive(false);
        looseScreen.SetActive(true);
    }

    public void GoBackToMenu()
    {
        FadeManager.instance.StartFadeOut(() => 
        {
            SceneManager.LoadScene(0);
        });
    }
}
