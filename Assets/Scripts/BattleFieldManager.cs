using System.Collections.Generic;
using Game;
using Game.Round;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public int GameFieldWidth = 10;
    public int GameFieldHeight = 10;
    public GameObject PlayerStateMachine;
    public List<GameObject> AllSingsOnSeBattlefield;

    public GameObject[,] _battlefield;

    private void Start()
    {
        _battlefield = new GameObject[GameFieldWidth, GameFieldHeight];

        AllSingsOnSeBattlefield.ForEach(element =>
        {
            var x = (int) element.transform.position.x;
            var y = (int) element.transform.position.y;
            _battlefield[x, y] = element;
        });

        PlayerStateMachine.GetComponent<PlayerGameStateMachine>().Battlefield = _battlefield;
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