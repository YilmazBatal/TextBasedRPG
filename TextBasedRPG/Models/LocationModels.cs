namespace TextBasedRPG.Models
{
    public class LocationData
    {
        public required string ID { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public int LevelCap { get; init; }
        public List<string>? AdventureTexts { get; init; }
        public List<Loots>? AdventureLoots { get; init; }
        public List<string>? Entities { get; init; }
    }
    public class Loots
    {
        public string ID { get; init; }
        public int DropChance { get; init; }
        public int MaxAmount { get; init; }
    }
}
