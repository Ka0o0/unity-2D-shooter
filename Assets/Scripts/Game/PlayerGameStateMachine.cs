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
            _gameRoundStateMachine.HandleEvent(gameEvent);

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
    }
}