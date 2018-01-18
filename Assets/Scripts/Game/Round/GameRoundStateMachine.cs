using System.Collections.Generic;
using Game.Round.GameEventHandler;
using Game.Round.GameStateObserver;
using UnityEngine;
using Utility.Tuple;

namespace Game.Round
{
    using StateMachineTransitionMap =
        Dictionary<Tuple<GameRoundState, GameRoundEventType>, System.Func<GameRoundEvent, GameRoundState>>;
    using StateMachineStateObserverList =
        LinkedList<Tuple<GameRoundState, System.Action<GameRoundEvent>>>;

    public class GameRoundStateMachine
    {
        public GameRoundState RoundState { get; private set; }

        private readonly StateMachineTransitionMap _stateMachineTransitionMap = new StateMachineTransitionMap();
        private readonly StateMachineStateObserverList _stateChangeObservers = new StateMachineStateObserverList();
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

            // Soldier Selection
            var soldierSelector = new SoldierSelector();
            _stateChangeObservers.AddLast(
                new Tuple<GameRoundState, System.Action<GameRoundEvent>>(GameRoundState.SoldierSelected,
                    soldierSelector.SoldierSelected)
            );
            _stateChangeObservers.AddLast(
                new Tuple<GameRoundState, System.Action<GameRoundEvent>>(GameRoundState.Idle, soldierSelector.Idling)
            );
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
                foreach (var observerTuple in _stateChangeObservers)
                {
                    if (observerTuple.Item1 == RoundState)
                    {
                        observerTuple.Item2(gameRoundEvent);
                    }
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