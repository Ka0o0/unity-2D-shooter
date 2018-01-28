namespace Game.Round.GameStateObserver
{
    public class SoldierSelector
    {

        private Soldier _selectedSoldier;

        public void SelectSoldier(Soldier _soldier)
        {
            _selectedSoldier = _soldier;
            _soldier.IsSelected = true;
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