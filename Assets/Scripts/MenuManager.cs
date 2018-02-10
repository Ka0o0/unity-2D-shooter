using System.Collections;
using System.Collections.Generic;
using Game;
using Game.Round;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject GameStateMachine;
    public GameObject SoldierActionGameMenuCanvas;
    public GameObject PlayerIdleMenuCanvas;
    public GameObject MainMenuCanvas;

    private bool _isMainMenuShown = false;

    public void ShowMainMenu()
    {
        _isMainMenuShown = true;
    }

    public void HideMainMenu()
    {
        _isMainMenuShown = false;
    }

    private void Update()
    {
        var stateMachine = GameStateMachine.GetComponent<PlayerGameStateMachine>();

        if (stateMachine.GameRoundStateMachine.RoundState == GameRoundState.SoldierAttack ||
            stateMachine.GameRoundStateMachine.RoundState == GameRoundState.SoldierMoving ||
            stateMachine.GameRoundStateMachine.RoundState == GameRoundState.SoldierMovement)
        {
            PlayerIdleMenuCanvas.SetActive(false);
            SoldierActionGameMenuCanvas.SetActive(false);
            MainMenuCanvas.SetActive(false);
        }
        else if (_isMainMenuShown)
        {
            PlayerIdleMenuCanvas.SetActive(false);
            SoldierActionGameMenuCanvas.SetActive(false);
            MainMenuCanvas.SetActive(true);
        }
        else if (stateMachine.GameRoundStateMachine.RoundState == GameRoundState.Idle)
        {
            SoldierActionGameMenuCanvas.SetActive(false);
            MainMenuCanvas.SetActive(false);
            PlayerIdleMenuCanvas.SetActive(true);
        }
        else
        {
            PlayerIdleMenuCanvas.SetActive(false);
            MainMenuCanvas.SetActive(false);
            SoldierActionGameMenuCanvas.SetActive(true);
        }
    }
}