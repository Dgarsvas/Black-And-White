using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 stairsPos;

    public Transform playerTransform;
    

    public delegate void Notify();
    public delegate void GoAfterThePlayer(Transform player);
    public Notify OnGameEnd;
    public GoAfterThePlayer OnGirlFound;
    public UIController uiController;

    private void Awake()
    {
        instance = this;
    }

    public void GirlFound()
    {
        uiController.ChangeObjective("Escape!");
        OnGirlFound?.Invoke(playerTransform);
    }

    public void PlayerEscaped()
    {
        OnGameEnd?.Invoke();
        uiController.ShowWinScreen();
    }

    public void PlayerDied()
    {
        OnGameEnd?.Invoke();
        uiController.ShowLooseScreen();
    }
}
