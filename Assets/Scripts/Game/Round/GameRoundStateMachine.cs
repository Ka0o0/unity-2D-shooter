using System;
using System.Collections.Generic;
using Game.Round.GameEventHandler;
using UnityEngine;
using Utility.Tuple;

namespace Game.Round
{
    using StateMachineTransitionMap =
        Dictionary<Tuple<GameRoundState, GameRoundEventType>, Func<GameRoundEvent, GameRoundState>>;

    public class GameRoundStateMachine
    {

        public GameRoundState RoundState { get; private set; }

        private readonly StateMachineTransitionMap  _stateMachineTransitionMap = new StateMachineTransitionMap();

        public GameRoundStateMachine()
        {
            RoundState = GameRoundState.Idle;
            
            var soldierMover = new SoldierMover();
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.Idle, GameRoundEventType.SoldierSelected),
                soldierMover.SelectSoldier);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierSelected,
                    GameRoundEventType.EmptyFieldSelected),
                soldierMover.MoveSoldier);
            
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.Idle, GameRoundEventType.FinishRound),
                FinishRound);
        }

        public void HandleEvent(GameRoundEvent gameRoundEvent)
        {
            var magic2 = new Tuple<GameRoundState, GameRoundEventType>(RoundState, gameRoundEvent.Type);
            if (_stateMachineTransitionMap.ContainsKey(magic2))
            {
                var oldState = RoundState;
                RoundState = _stateMachineTransitionMap[magic2](gameRoundEvent);
                if (oldState != RoundState)
                {
                    Debug.Log("State changed to " + RoundState);
                }
            }
        }

        private GameRoundState FinishRound(GameRoundEvent gameRoundEvent)
        {
            return GameRoundState.Finished;
        }
    }
}