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

        private readonly StateMachineTransitionMap _stateMachineTransitionMap = new StateMachineTransitionMap();
        private readonly SoldierMover _soldierMover = new SoldierMover();
        private readonly SoldierShooter _soldierShooter = new SoldierShooter();

        public GameRoundStateMachine()
        {
            RoundState = GameRoundState.Idle;

            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.Idle, GameRoundEventType.SoldierSelected),
                SelectSoldier);

            // Movement
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierSelected,
                    GameRoundEventType.MovingModeStarted),
                _soldierMover.StartMovementSate);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierMovement,
                    GameRoundEventType.EmptyFieldSelected),
                _soldierMover.MoveSoldier);
            
            // Shooting
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierSelected,
                    GameRoundEventType.ShootingModeStarted),
                _soldierShooter.StartShootingState);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierAttack,
                    GameRoundEventType.EnemySelected),
                _soldierShooter.ShootEnemy);

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

        private GameRoundState SelectSoldier(GameRoundEvent gameRoundEvent)
        {
            var soldier = gameRoundEvent.Payload as Soldier;
            if (soldier)
            {
                _soldierMover.SelectSoldier(soldier);
                _soldierShooter.SelectSoldier(soldier);
                
                return GameRoundState.SoldierSelected;
            }
            return GameRoundState.Idle;
        }
    }
}