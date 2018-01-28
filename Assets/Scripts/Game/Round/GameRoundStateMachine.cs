using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
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
        private GameObject[,] _battlefield;

        public GameObject[,] Battlefield
        {
            private get { return _battlefield; }
            set
            {
                _soldierMover.Battlefield = value;
                _battlefield = value;
            }
        }

        private readonly StateMachineTransitionMap _stateMachineTransitionMap = new StateMachineTransitionMap();
        private readonly StateMachineStateObserverList _stateChangeObservers = new StateMachineStateObserverList();
        private readonly SoldierMover _soldierMover = new SoldierMover();
        private readonly SoldierShooter _soldierShooter = new SoldierShooter();
        private readonly SoldierSelector _soldierSelector = new SoldierSelector();

        public GameRoundStateMachine()
        {
            RoundState = GameRoundState.Idle;

            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.Idle, GameRoundEventType.FieldSelected),
                SelectSoldier);

            // Movement
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierSelected,
                    GameRoundEventType.MovingModeStarted),
                _soldierMover.StartMovementSate);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierMovement,
                    GameRoundEventType.FieldSelected),
                _soldierMover.MoveSoldier);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierMoving,
                    GameRoundEventType.SoldierFinishedMovement),
                _soldierMover.FinishMovement);


            // Shooting
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierSelected,
                    GameRoundEventType.ShootingModeStarted),
                _soldierShooter.StartShootingState);
            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.SoldierAttack,
                    GameRoundEventType.FieldSelected),
                _soldierShooter.ShootEnemy);

            _stateMachineTransitionMap.Add(
                new Tuple<GameRoundState, GameRoundEventType>(GameRoundState.Idle, GameRoundEventType.FinishRound),
                FinishRound);

            // Soldier Selection
            _stateChangeObservers.AddLast(
                new Tuple<GameRoundState, System.Action<GameRoundEvent>>(GameRoundState.Idle, _soldierSelector.Idling)
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
            var position = (Vector2) gameRoundEvent.Payload;

            var go = Battlefield[(int) position.x, (int) position.y];
            if (!go) return GameRoundState.Idle;

            var soldier = go.GetComponent<Soldier>();
            if (!soldier) return GameRoundState.Idle;

            _soldierMover.SelectSoldier(soldier);
            _soldierShooter.SelectSoldier(soldier);
            _soldierSelector.SelectSoldier(soldier);

            return GameRoundState.SoldierSelected;
        }
    }
}