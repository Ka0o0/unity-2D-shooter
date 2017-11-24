using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Game : MonoBehaviour
    {
        private GameState _state = GameState.Idle;
        private readonly Dictionary<Tuple<GameState, GameEvent.GameEventType>, Func<GameEvent, GameState>> _stateMachineTransitionMap = new Dictionary<Tuple<GameState, GameEvent.GameEventType>, Func<GameEvent, GameState>>();

        public Game()
        {
            _stateMachineTransitionMap.Add(new Tuple<GameState, GameEvent.GameEventType>(), );
        }
        
        public void HandleEvent(GameEvent gameEvent)
        {
            var magic2 = new Tuple<GameState, GameEvent.GameEventType>(_state, gameEvent.Type);
            if (_stateMachineTransitionMap.ContainsKey(magic2))
            {
                var newState = _stateMachineTransitionMap[magic2](gameEvent);
                if (newState != GameState.Invalid)
                {
                    _state = newState;
                }
            }
        }
    }
}