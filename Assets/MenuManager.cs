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

	private void Update()
	{
		var stateMachine = GameStateMachine.GetComponent<PlayerGameStateMachine>();
		if (stateMachine.GameRoundStateMachine.RoundState == GameRoundState.Idle)
		{
			PlayerIdleMenuCanvas.SetActive(true);
			SoldierActionGameMenuCanvas.SetActive(false);
		}
		else
		{
			PlayerIdleMenuCanvas.SetActive(false);
			SoldierActionGameMenuCanvas.SetActive(true);
		}
	}
}
