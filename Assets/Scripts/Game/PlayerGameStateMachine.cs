using System.Collections.Generic;
using Game.Round;
using UnityEngine;

namespace Game
{
    public class PlayerGameStateMachine: MonoBehaviour
    {
        private PlayerGameState State;
        public GameRoundStateMachine GameRoundStateMachine;
        public List<GameObject> Soldiers;

        private void Start()
        {
            State = PlayerGameState.AlicesRound;
            CreateNewGameRoundStateMachine();
            UpdateSoldiersTeamSelectionState();
        }

        public void HandleEvent(GameRoundEvent gameEvent)
        {
            if (gameEvent.Type == GameRoundEventType.SoldierSelected)
            {
                var soldier = gameEvent.Payload as Soldier;

                if (IsSoldierEnemy(soldier))
                {
                    GameRoundStateMachine.HandleEvent(new EnemySelectedGameRoundeEvent(soldier));
                }
                else
                {
                    GameRoundStateMachine.HandleEvent(gameEvent);
                }
            }
            else
            {
                GameRoundStateMachine.HandleEvent(gameEvent);
            }

            if (GameRoundStateMachine.RoundState == GameRoundState.Finished)
            {
                SwapState();
                CreateNewGameRoundStateMachine();
            }
        }

        private void CreateNewGameRoundStateMachine()
        {
            GameRoundStateMachine = new GameRoundStateMachine();
        }

        private void SwapState()
        {
            State = State == PlayerGameState.AlicesRound ? PlayerGameState.BobsRound : PlayerGameState.AlicesRound;
            Debug.Log("Player's round switched to " + State);
            UpdateSoldiersTeamSelectionState();
        }

        private bool IsSoldierEnemy(Soldier soldier)
        {
            return (State == PlayerGameState.BobsRound && soldier.Team == Team.TeamAlice) ||
                   (State == PlayerGameState.AlicesRound && soldier.Team == Team.TeamBob);
        }

        private void UpdateSoldiersTeamSelectionState()
        {
            foreach (var goSoldier in Soldiers)
            {
                var soldier = goSoldier.GetComponent<Soldier>();
                soldier.IsTeamActive = !IsSoldierEnemy(soldier);
            }
        }
    }
}