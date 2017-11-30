using Game;
using Game.Round;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    public int GameFieldWidth = 10;
    public int GameFieldHeight = 10;

    private PlayerGameStateMachine _gameStateMachine;

    private void Start()
    {
        _gameStateMachine = new PlayerGameStateMachine(PlayerGameState.AlicesRound);
    }

    private void Update()
    {
        var gameEvent = ReadGameEventFromUserInput();
        if (gameEvent != null)
        {
            _gameStateMachine.HandleEvent(gameEvent);
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
}