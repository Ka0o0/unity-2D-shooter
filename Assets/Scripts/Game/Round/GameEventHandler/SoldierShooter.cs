using System.Collections.Generic;

namespace Game.Round.GameEventHandler
{
    public class SoldierShooter
    {
        private Soldier _selectedSoldier;
        private readonly Dictionary<Soldier, int> SoldierShootingCountMap = new Dictionary<Soldier, int>();

        public void SelectSoldier(Soldier soldier)
        {
            _selectedSoldier = soldier;
        }

        public GameRoundState StartShootingState(GameRoundEvent gameRoundEvent)
        {
            if (_selectedSoldier == null)
            {
                return GameRoundState.Idle;
            }
            
            InitSoldierShootingMapFor(_selectedSoldier);
            if (SoldierShootingCountMap[_selectedSoldier] >= _selectedSoldier.MaxShootingsCount)
            {
                return GameRoundState.SoldierSelected;
            }
            

            return GameRoundState.SoldierAttack;
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

            InitSoldierShootingMapFor(soldier);
            SoldierShootingCountMap[_selectedSoldier]++;
            
            return GameRoundState.Idle;
        }

        private void InitSoldierShootingMapFor(Soldier _soldier)
        {
            if (!SoldierShootingCountMap.ContainsKey(_soldier))
            {
                SoldierShootingCountMap[_soldier] = 0;
            }
        }
    }
}