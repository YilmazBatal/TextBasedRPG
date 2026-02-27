namespace TextBasedRPG.Models
{
    public class WeaponData
    {
        public required string ID { get; init; }
        public string? ItemType { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int Price { get; init; }
        public string? Rarity { get; init; }
        public int Quantity { get; init; }
        public int WeaponATK { get; init; }
        public string? WeaponType { get; init; }
        public int RequiredLevel { get; init; }
    }
    public class ArmorData
    {
        public required string ID { get; init; }
        public string? ItemType { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int Price { get; init; }
        public string? Rarity { get; init; }
        public int Quantity { get; init; }
        public int ArmorDef { get; init; }
        public int ExtraHP { get; init; }
        public int RequiredLevel { get; init; }
    }
    public class MaterialData
    {
        public required string ID { get; init; }
        public string? ItemType { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int Price { get; init; }
        public string? Rarity { get; init; }
        public int Quantity { get; init; }
        public int MaxQuantity { get; init; }
    }
    public class ConsumableData
    {
        public required string ID { get; init; }
        public string? ItemType { get; init; }
        public string? Name { get; init; }
        public string? Description { get; init; }
        public int Price { get; init; }
        public string? Rarity { get; init; }
        public int Quantity { get; init; }
        public string? Effect { get; init; }
        public int Value { get; init; }
        public bool CombatItem { get; init; }
    }
}
