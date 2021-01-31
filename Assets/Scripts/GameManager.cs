using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform playerTransform;
    public Vector3 stairsPos;

    public delegate void Notify();
    public delegate void GoAfterThePlayer(Transform player);
    public Notify OnGameEnd;
    public GoAfterThePlayer OnGirlFound;
    public UIController uiController;

    private void Awake()
    {
        uiController.ChangeObjective("Find the girl!");
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
