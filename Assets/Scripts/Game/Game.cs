using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.Tuple;

namespace Game
{
    public class GameMachine : MonoBehaviour
    {
        private GameState _state = GameState.Idle;

        private readonly Dictionary<Tuple<GameState, GameEventType>, Func<GameEvent, GameState>>
            _stateMachineTransitionMap = new Dictionary<Tuple<GameState, GameEventType>, Func<GameEvent, GameState>>();

        public GameMachine()
        {
            var SoldierMover = new SoldierMover();
            _stateMachineTransitionMap.Add(
                new Tuple<GameState, GameEventType>(GameState.Idle, GameEventType.SoldierSelected),
                SoldierMover.SelectSoldier);
            _stateMachineTransitionMap.Add(
                new Tuple<GameState, GameEventType>(GameState.SoldierSelected, GameEventType.EmptyFieldSelected),
                SoldierMover.MoveSoldier);
        }

        public void HandleEvent(GameEvent gameEvent)
        {
            var magic2 = new Tuple<GameState, GameEventType>(_state, gameEvent.Type);
            if (_stateMachineTransitionMap.ContainsKey(magic2))
            {
                var oldState = _state;
                _state = _stateMachineTransitionMap[magic2](gameEvent);
                if (oldState != _state)
                {
                    print("State changed to " + _state.ToString());
                }
            }
        }
    }
}