using UnityEngine;

namespace Game.Round
{
    public class GameRoundEvent
    {
        
        public readonly GameRoundEventType Type;
        public readonly object Payload;

        protected GameRoundEvent(GameRoundEventType type, object payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    public class SoldierSelectedGameRoundEvent: GameRoundEvent
    {
        public SoldierSelectedGameRoundEvent(Soldier payload) : base(GameRoundEventType.SoldierSelected, payload)
        {
        }
    }
    
    public class EmptyFieldSelectedGameRoundEvent: GameRoundEvent
    {
        public EmptyFieldSelectedGameRoundEvent(Vector2 payload) : base(GameRoundEventType.EmptyFieldSelected, payload)
        {
        }
    }
}