using System.Collections.Generic;
using System.Linq;
using Game.Round;
using UnityEngine;

namespace Game
{
    public class PlayerGameStateMachine : MonoBehaviour
    {
        private PlayerGameState State;
        public GameRoundStateMachine GameRoundStateMachine;
        public List<GameObject> Soldiers;

        private GameObject[,] _battlefield;

        public GameObject[,] Battlefield
        {
            private get { return _battlefield; }
            set
            {
                if (GameRoundStateMachine != null)
                {
                    GameRoundStateMachine.Battlefield = value;
                }

                _battlefield = value;
            }
        }

        private void Start()
        {
            State = PlayerGameState.AlicesRound;
            CreateNewGameRoundStateMachine();
            UpdateSoldiersTeamSelectionState();
        }

        public void HandleEvent(GameRoundEvent gameEvent)
        {
            GameRoundStateMachine.HandleEvent(gameEvent);

            if (GameRoundStateMachine.RoundState == GameRoundState.Finished)
            {
                SwapState();
                CreateNewGameRoundStateMachine();
            }
        }

        private void CreateNewGameRoundStateMachine()
        {
            GameRoundStateMachine = new GameRoundStateMachine
            {
                Battlefield = Battlefield,
                OwnSoldiers = Soldiers.Where(s => !IsSoldierEnemy(s.GetComponent<Soldier>())).ToList()
            };
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
                // Reset Walking Distance
                soldier.MovementManager.WalkingDistanceInCurrentRound = 0;
            }
        }
    }
}