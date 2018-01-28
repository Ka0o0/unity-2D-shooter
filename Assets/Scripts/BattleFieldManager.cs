﻿using System.Collections.Generic;
using Game;
using Game.Round;
using UnityEditor;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public int GameFieldWidth = 10;
    public int GameFieldHeight = 10;
    public GameObject PlayerStateMachine;
    public GameObject BattleFieldBlockPrefab;

    public GameObject[][] BattleFieldBlocks;

    private void Start()
    {
        BattleFieldBlocks = new GameObject[GameFieldWidth][];
        for (int i = 0; i < GameFieldWidth; i++)
        {
            BattleFieldBlocks[i] = new GameObject[GameFieldHeight];
            for (int j = 0; j < GameFieldHeight; j++)
            {
                var block = Instantiate(BattleFieldBlockPrefab);
                var x = i + (float) 0.5; // Offset
                var y = j + (float) 0.5;
                block.transform.position = new Vector3(x, y, 0);
                BattleFieldBlocks[i][j] = block;
            }
        }
    }

    private void Update()
    {
        var gameEvent = ReadGameEventFromUserInput();
        if (gameEvent != null)
        {
            PlayerStateMachine.GetComponent<PlayerGameStateMachine>().HandleEvent(gameEvent);
        }
    }

    private GameRoundEvent ReadGameEventFromUserInput()
    {
        var gameEvent = ReadGameEventFromMouseInput();
        if (gameEvent != null)
        {
            return gameEvent;
        }

        return null;
    }

    private GameRoundEvent ReadGameEventFromMouseInput()
    {
        if (!Input.GetMouseButtonDown(0)) return null;
        
        var camera = Camera.main;
        var clickedPositionInWorld = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,
            camera.ScreenToWorldPoint(Input.mousePosition).y);
        var hit = Physics2D.Raycast(clickedPositionInWorld, Vector2.zero);
        if (hit)
        {
            var selectedGameObject = hit.collider.gameObject;
            if (selectedGameObject.CompareTag("Player"))
            {
                return new SoldierSelectedGameRoundEvent(selectedGameObject.GetComponent<Soldier>());
            }
        }
        else if (PointIsInsideGameField(clickedPositionInWorld))
        {
            return new EmptyFieldSelectedGameRoundEvent(clickedPositionInWorld);
        }

        return null;
    }

    private bool PointIsInsideGameField(Vector2 point)
    {
        return point.x >= 0 &&
               point.x < GameFieldWidth &&
               point.y >= 0 &&
               point.y < GameFieldHeight;
    }

    public void ShootButtonTapped()
    {
        PlayerStateMachine.GetComponent<PlayerGameStateMachine>().HandleEvent(new AttackModeStartedGameRoundeEvent());
    }
    
    public void MoveButtonTapped()
    {
        PlayerStateMachine.GetComponent<PlayerGameStateMachine>().HandleEvent(new MovingModeStartedGameRoundeEvent());
    }

    public void FinishButtonTapped()
    {
        PlayerStateMachine.GetComponent<PlayerGameStateMachine>().HandleEvent(new FinishRoundEvent());
    }
}