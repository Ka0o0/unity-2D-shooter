using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Round;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public int GameFieldWidth = 10;
    public int GameFieldHeight = 10;
    public GameObject PlayerStateMachine;
    public GameObject BattleFieldBlockPrefab;
    public GameObject[,] BattleFieldBlocks;
    public List<GameObject> AllSingsOnSeBattlefield;
    public GameObject[,] BattleField;

    private void Start()
    {
        BattleFieldBlocks = new GameObject[GameFieldWidth, GameFieldHeight];
        for (var i = 0; i < GameFieldWidth; i++)
        {
            for (var j = 0; j < GameFieldHeight; j++)
            {
                var block = Instantiate(BattleFieldBlockPrefab);
                var x = i + (float) 0.5; // Offset
                var y = j + (float) 0.5;
                block.transform.position = new Vector3(x, y, 0);
                BattleFieldBlocks[i, j] = block;
            }
        }

        AllSingsOnSeBattlefield.Where(sing => sing.CompareTag("Wall"))
            .ToList()
            .ForEach(wall =>
            {
                var wallRenderer = wall.GetComponent<SpriteRenderer>();
                var bounds = wallRenderer.bounds;

                if (bounds.size.x > bounds.size.y)
                {
                    var hLine = (int) Math.Round(bounds.center.y);
                    var start = (int) Math.Ceiling(bounds.min.x);
                    var end = (int) Math.Floor(bounds.max.x);

                    for (var i = start; i < end; i++)
                    {
                        BattleFieldBlocks[i, hLine - 1].GetComponent<BattleFieldBlock>().HasTopWall = true;
                        BattleFieldBlocks[i, hLine].GetComponent<BattleFieldBlock>().HasBottomWall = true;
                    }
                }
                else
                {
                    var vLine = (int) Math.Round(bounds.center.x);
                    var start = (int) Math.Ceiling(bounds.min.y);
                    var end = (int) Math.Floor(bounds.max.y);

                    for (var i = start; i < end; i++)
                    {
                        BattleFieldBlocks[vLine - 1, i].GetComponent<BattleFieldBlock>().HasRightWall = true;
                        BattleFieldBlocks[vLine, i].GetComponent<BattleFieldBlock>().HasLeftWall = true;
                    }
                }
            });

        BattleField = new GameObject[GameFieldWidth, GameFieldHeight];
        var tags = new[] {"Player", "Obstacle"};
        AllSingsOnSeBattlefield.Where(sing => tags.Any(sing.CompareTag))
            .ToList()
            .ForEach(element =>
            {
                var x = (int) element.transform.position.x;
                var y = (int) element.transform.position.y;
                BattleField[x, y] = element;
            });

        PlayerStateMachine.GetComponent<PlayerGameStateMachine>().Battlefield = BattleField;
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

        if (PointIsInsideGameField(clickedPositionInWorld))
        {
            return new FieldSelectedGameRoundEvent(clickedPositionInWorld);
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