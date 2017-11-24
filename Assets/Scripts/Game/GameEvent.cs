namespace Game
{
    public class GameEvent
    {
        public enum GameEventType
        {
            Test
        }

        public GameEventType Type;
        public object Payload;

        public GameEvent(GameEventType type, object payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}