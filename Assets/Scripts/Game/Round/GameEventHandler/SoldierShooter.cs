namespace Game.Round.GameEventHandler
{
    public class SoldierShooter
    {
        private Soldier _selectedSoldier;

        public void SelectSoldier(Soldier soldier)
        {
            _selectedSoldier = soldier;
        }

        public GameRoundState StartShootingState(GameRoundEvent gameRoundEvent)
        {
            return gameRoundEvent.Type == GameRoundEventType.ShootingModeStarted && _selectedSoldier != null
                ? GameRoundState.SoldierAttack
                : GameRoundState.Idle;
        }

        public GameRoundState ShootEnemy(GameRoundEvent gameRoundEvent)
        {
            if (gameRoundEvent.Type != GameRoundEventType.EnemySelected)
            {
                return GameRoundState.Idle;
            }

            var enemySoldier = gameRoundEvent.Payload as Soldier;
            var soldier = _selectedSoldier;

            if (!(enemySoldier && soldier))
            {
                return GameRoundState.Idle;
            }
            
            soldier.ShootToPoint(enemySoldier.transform.position);

            return GameRoundState.Finished;
        }
    }
}