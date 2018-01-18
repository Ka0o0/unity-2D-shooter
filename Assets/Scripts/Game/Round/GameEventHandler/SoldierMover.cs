using UnityEngine;

namespace Game.Round.GameEventHandler
{
    public class SoldierMover
    {
        private Soldier _selectedSoldier;

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
            
            if (gameRoundEvent.Type == GameRoundEventType.EmptyFieldSelected)
            {
                var position = (Vector2) gameRoundEvent.Payload;
                // Keep the current state (SoldierSelected) when the soldier can't move to a certain position
                if (!_selectedSoldier.CanMoveToPosition(position))
                {
                    return GameRoundState.SoldierSelected;
                }
                
                _selectedSoldier.MoveToPosition(position);
            }
            
            return GameRoundState.Idle;
        }
    }
}