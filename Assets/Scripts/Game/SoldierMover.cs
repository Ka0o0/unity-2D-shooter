using UnityEngine;

namespace Game
{
    public class SoldierMover
    {
        private Soldier _selectedSoldier;

        public GameState SelectSoldier(GameEvent gameEvent)
        {
            if (gameEvent.Type == GameEventType.SoldierSelected)
            {
                // This can be null
                _selectedSoldier = gameEvent.Payload as Soldier;
                
                if (_selectedSoldier)
                {
                    return GameState.SoldierSelected;
                }
            }
            return GameState.Idle;
        }

        public GameState MoveSoldier(GameEvent gameEvent)
        {
            if (_selectedSoldier == null)
            {
                return GameState.Idle;
            }
            
            if (gameEvent.Type == GameEventType.EmptyFieldSelected)
            {
                var position = (Vector2) gameEvent.Payload;
                // Keep the current state (SoldierSelected) when the soldier can't move to a certain position
                if (!_selectedSoldier.CanMoveToPosition(position))
                {
                    return GameState.SoldierSelected;
                }
                
                _selectedSoldier.MoveToPosition(position);
            }
            
            return GameState.Idle;
        }
    }
}