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

    public class FieldSelectedGameRoundEvent: GameRoundEvent
    {
        public FieldSelectedGameRoundEvent(Vector2 payload) : base(GameRoundEventType.FieldSelected, payload)
        {
        }
    }
    
    public class MovingModeStartedGameRoundeEvent : GameRoundEvent
    {
        public MovingModeStartedGameRoundeEvent() : base(GameRoundEventType.MovingModeStarted, null)
        {
        }
    }
    
    public class AttackModeStartedGameRoundeEvent : GameRoundEvent
    {
        public AttackModeStartedGameRoundeEvent() : base(GameRoundEventType.ShootingModeStarted, null)
        {
        }
    }
    
    public class FinishRoundEvent : GameRoundEvent
    {
        public FinishRoundEvent() : base(GameRoundEventType.FinishRound, null)
        {
        }
    }
}