using UnityEngine;

namespace Game.Round.GameEventHandler
{
    public class SoldierMover
    {
        private Soldier _selectedSoldier;

        public GameRoundState SelectSoldier(GameRoundEvent gameRoundEvent)
        {
            if (gameRoundEvent.Type == GameRoundEventType.SoldierSelected)
            {
                // This can be null
                _selectedSoldier = gameRoundEvent.Payload as Soldier;
                
                if (_selectedSoldier)
                {
                    return GameRoundState.SoldierSelected;
                }
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
                // TODO only for testing
                return GameRoundState.Finished;
            }
            
            return GameRoundState.Idle;
        }
    }
}