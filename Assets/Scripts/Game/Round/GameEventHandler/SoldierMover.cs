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

        public GameRoundState MoveSoldier(GameRoundEvent gameRoundEvent)
        {
            if (_selectedSoldier == null)
            {
                return GameRoundState.Idle;
            }

            if (gameRoundEvent.Type == GameRoundEventType.FieldSelected)
            {
                var position = (Vector2) gameRoundEvent.Payload;
                // Keep the current state (SoldierSelected) when the soldier can't move to a certain position
                if (!CanMoveToPosition(position))
                {
                    return GameRoundState.SoldierSelected;
                }

                var oldPosition = _selectedSoldier.transform.position;
                Battlefield[(int) position.x, (int) position.y] = Battlefield[(int) oldPosition.x, (int) oldPosition.y];
                Battlefield[(int) oldPosition.x, (int) oldPosition.y] = null;

                _selectedSoldier.MoveToPosition(position);
            }

            return GameRoundState.Idle;
        }

        private bool CanMoveToPosition(Vector2 position)
        {
            return Battlefield[(int) position.x, (int) position.y] == null;
        }
    }
}