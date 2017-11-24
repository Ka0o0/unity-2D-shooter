using NUnit.Framework.Constraints;
using UnityEngine;

namespace Game
{
    public class GameEvent
    {
        public GameEventType Type;
        public object Payload;

        protected GameEvent(GameEventType type, object payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    public class SoldierSelectedGameEvent: GameEvent
    {
        public SoldierSelectedGameEvent(Soldier payload) : base(GameEventType.SoldierSelected, payload)
        {
        }
    }
    
    public class EmptyFieldSelectedGameEvent: GameEvent
    {
        public EmptyFieldSelectedGameEvent(Vector2 payload) : base(GameEventType.EmptyFieldSelected, payload)
        {
        }
    }
}