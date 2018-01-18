namespace Game.Round.GameStateObserver
{
    public class SoldierSelector
    {

        private Soldier _selectedSoldier;

        public void SoldierSelected(GameRoundEvent _event)
        {
            _selectedSoldier = _event.Payload as Soldier;
            if (_selectedSoldier != null)
            {
                _selectedSoldier.IsSelected = true;
            }
        }

        public void Idling(GameRoundEvent _event)
        {
            if (_selectedSoldier == null)
            {
                return;
            }

            _selectedSoldier.IsSelected = false;
        }
        
    }
}