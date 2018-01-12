using Game.Round;
using UnityEngine;

namespace Game
{
    public class PlayerGameStateMachine
    {
        private PlayerGameState _state;
        private GameRoundStateMachine _gameRoundStateMachine;

        public PlayerGameStateMachine(PlayerGameState initialState)
        {
            _state = initialState;
            CreateNewGameRoundStateMachine();
        }

        public void HandleEvent(GameRoundEvent gameEvent)
        {
            if (gameEvent.Type == GameRoundEventType.SoldierSelected)
            {
                var soldier = gameEvent.Payload as Soldier;

                if (isSoldierEnemy(soldier))
                {
                    _gameRoundStateMachine.HandleEvent(new EnemySelectedGameRoundeEvent(soldier));
                }
                else
                {
                    _gameRoundStateMachine.HandleEvent(gameEvent);
                }
            }
            else
            {
                _gameRoundStateMachine.HandleEvent(gameEvent);
            }

            if (_gameRoundStateMachine.RoundState == GameRoundState.Finished)
            {
                SwapState();
                CreateNewGameRoundStateMachine();
            }
        }

        private void CreateNewGameRoundStateMachine()
        {
            _gameRoundStateMachine = new GameRoundStateMachine();
        }

        private void SwapState()
        {
            _state = _state == PlayerGameState.AlicesRound ? PlayerGameState.BobsRound : PlayerGameState.AlicesRound;
            Debug.Log("Player's round switched to " + _state);
        }

        private bool isSoldierEnemy(Soldier soldier)
        {
            return (_state == PlayerGameState.BobsRound && soldier.Team == Team.TeamAlice) ||
                   (_state == PlayerGameState.AlicesRound && soldier.Team == Team.TeamBob);
        }
    }
}