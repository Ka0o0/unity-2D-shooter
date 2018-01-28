using UnityEngine;

namespace Game.Round.GameEventHandler
{
    public class SoldierMover
    {
        private Soldier _selectedSoldier;
        public GameObject[,] Battlefield;

        public void SelectSoldier(Soldier soldier)
        {
            _selectedSoldier = soldier;
        }

        public GameRoundState StartMovementSate(GameRoundEvent gameRoundEvent)
        {
            if (gameRoundEvent.Type == GameRoundEventType.MovingModeStarted && _selectedSoldier != null)
            {
                return GameRoundState.SoldierMovement;
            }
            return GameRoundState.Idle;
        }

        public GameRoundState FinishMovement(GameRoundEvent gameRoundEvent)
        {
            _selectedSoldier = null;
            return GameRoundState.Idle;
        }

        public GameRoundState MoveSoldier(GameRoundEvent gameRoundEvent)
        {
            if (_selectedSoldier == null)
            {
                return GameRoundState.Idle;
            }

            if (gameRoundEvent.Type == GameRoundEventType.FieldSelected)
            {
                var position = (Vector2) gameRoundEvent.Payload;
                var targetPosition = new Vector2Int(
                    (int) position.x,
                    (int) position.y
                );

                var targetPath = GetTargetPathToPosition(targetPosition);
                if (targetPath == null)
                {
                    return GameRoundState.SoldierSelected;
                }

                // Keep the Battlefield objects up to date
                var oldPosition = _selectedSoldier.transform.position;
                Battlefield[(int) position.x, (int) position.y] = Battlefield[(int) oldPosition.x, (int) oldPosition.y];
                Battlefield[(int) oldPosition.x, (int) oldPosition.y] = null;


                _selectedSoldier.MovementManager.MoveToPositionUsingPath(targetPosition, targetPath);
                return _selectedSoldier.MovementManager.IsMoving ? GameRoundState.SoldierMoving : GameRoundState.Idle;
            }

            return GameRoundState.Idle;
        }

        private Vector2Int[] GetTargetPathToPosition(Vector2Int position)
        {
            var movementManager = _selectedSoldier.MovementManager;
            var pathFinder = movementManager.MakeNewPathFinder();
            var possiblePaths = pathFinder.GetReachablePaths();
            return possiblePaths.ContainsKey(position) ? possiblePaths[position] : null;
        }
    }
}