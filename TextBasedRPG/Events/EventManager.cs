namespace TextBasedRPG.Events
{
    public static class EventManager
    {
        public static class Combat
        {
            public static event Action? OnRoundEnded;
            public static void TriggerOnRoundEnded() => OnRoundEnded?.Invoke();
        }
        public static class Game
        {
            public static event Action<int>? OnGoldChanged;
            public static void TriggerGoldChanged(int amount) => OnGoldChanged?.Invoke(amount);
        }
    }
}
