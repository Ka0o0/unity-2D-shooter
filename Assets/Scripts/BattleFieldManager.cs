using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class BattleFieldManager : MonoBehaviour
{
    public int GameFieldWidth = 10;
    public int GameFieldHeight = 10;

    private GameMachine Game;

    private void Start()
    {
        Game = new GameMachine();
    }

    private void Update()
    {
        var gameEvent = ReadGameEventFromUserInput();
        if (gameEvent != null)
        {
            Game.HandleEvent(gameEvent);
        }
    }

    private GameEvent ReadGameEventFromUserInput()
    {
        var gameEvent = ReadGameEventFromMouseInput();
        if (gameEvent != null)
        {
            return gameEvent;
        }

        return null;
    }

    private GameEvent ReadGameEventFromMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var camera = Camera.main;
            var magic = new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x,
                camera.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(magic, Vector2.zero);
            if (hit)
            {
                var selectedGameObject = hit.collider.gameObject;
                if (selectedGameObject.CompareTag("Player"))
                {
                    return new SoldierSelectedGameEvent(selectedGameObject.GetComponent<Soldier>());
                }
            }
            else if (PointIsInsideGameField(hit.point))
            {
                return new EmptyFieldSelectedGameEvent(magic);
            }
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